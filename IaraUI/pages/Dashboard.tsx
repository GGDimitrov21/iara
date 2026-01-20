import React, { useState, useEffect } from 'react';
import { Layout } from '../components/Layout';
import { Ship, FileText, Anchor, ArrowUpRight, ArrowDownRight, AlertCircle, Loader2 } from 'lucide-react';
import { Link } from 'react-router-dom';
import { vesselsApi, permitsApi, logbookApi, inspectionsApi, VesselDto, PermitDto, LogbookEntryDto, InspectionDto } from '../services/api';

const StatCard = ({ title, value, change, changeType, icon: Icon, loading }: any) => (
  <div className="bg-white p-6 rounded-xl border border-gray-100 shadow-sm relative overflow-hidden">
    <div className="flex justify-between items-start z-10 relative">
      <div>
        <p className="text-sm font-medium text-gray-500">{title}</p>
        <h3 className="text-3xl font-bold text-gray-900 mt-2">
          {loading ? <Loader2 className="w-8 h-8 animate-spin text-gray-400" /> : value}
        </h3>
      </div>
      <div className={`p-2 rounded-lg ${changeType === 'increase' ? 'bg-green-50 text-green-600' : 'bg-red-50 text-red-600'}`}>
         {changeType === 'increase' ? <ArrowUpRight className="w-5 h-5" /> : <ArrowDownRight className="w-5 h-5" />}
      </div>
    </div>
    <div className={`mt-2 text-sm font-medium ${changeType === 'increase' ? 'text-green-600' : 'text-red-600'}`}>
      {change}
    </div>
    
    {/* Decorative background icon */}
    <Icon className="absolute -bottom-4 -right-4 w-32 h-32 text-gray-50 opacity-50" />
  </div>
);

interface DashboardStats {
  totalVessels: number;
  activePermits: number;
  recentOperations: number;
}

interface RecentActivity {
  id: number;
  vesselName: string;
  type: string;
  date: string;
  status: string;
  statusColor: string;
}

export const Dashboard: React.FC = () => {
  const [stats, setStats] = useState<DashboardStats>({ totalVessels: 0, activePermits: 0, recentOperations: 0 });
  const [recentActivities, setRecentActivities] = useState<RecentActivity[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    loadDashboardData();
  }, []);

  const loadDashboardData = async () => {
    try {
      setLoading(true);
      setError(null);

      // Fetch all data in parallel
      const [vessels, permits, logbookEntries, inspections] = await Promise.all([
        vesselsApi.getAll().catch(() => [] as VesselDto[]),
        permitsApi.getAll().catch(() => [] as PermitDto[]),
        logbookApi.getAll().catch(() => [] as LogbookEntryDto[]),
        inspectionsApi.getAll().catch(() => [] as InspectionDto[])
      ]);

      // Calculate stats
      const activePermits = permits.filter(p => p.isActive).length;
      
      setStats({
        totalVessels: vessels.length,
        activePermits: activePermits,
        recentOperations: logbookEntries.length
      });

      // Combine logbook entries and inspections for recent activities
      const activities: RecentActivity[] = [];

      // Add recent logbook entries
      logbookEntries.slice(0, 5).forEach(entry => {
        activities.push({
          id: entry.logEntryId,
          vesselName: entry.vesselName,
          type: `Caught ${entry.catchKg?.toFixed(1) || 0} kg of ${entry.speciesName}`,
          date: new Date(entry.startTime).toLocaleDateString(),
          status: 'Completed',
          statusColor: 'bg-green-100 text-green-800'
        });
      });

      // Add recent inspections
      inspections.slice(0, 5).forEach(inspection => {
        activities.push({
          id: inspection.inspectionId + 10000, // Offset to avoid ID conflicts
          vesselName: inspection.vesselName,
          type: `Inspection by ${inspection.inspectorName}`,
          date: new Date(inspection.inspectionDate).toLocaleDateString(),
          status: inspection.isLegal ? 'Compliant' : 'Violation',
          statusColor: inspection.isLegal ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'
        });
      });

      // Sort by date (most recent first) and take top 5
      activities.sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime());
      setRecentActivities(activities.slice(0, 5));

    } catch (err) {
      console.error('Failed to load dashboard data:', err);
      setError('Failed to load dashboard data');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Layout>
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-gray-900">Dashboard</h1>
        <p className="text-gray-500 mt-2">Welcome back, Personnel! Here's an overview of your fishing activities.</p>
      </div>

      {error && (
        <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg mb-6 flex items-center gap-2">
          <AlertCircle className="w-5 h-5" />
          {error}
        </div>
      )}

      {/* Stats Grid */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
        <StatCard 
          title="Total Vessels" 
          value={stats.totalVessels} 
          change={stats.totalVessels > 0 ? "Active fleet" : "No vessels"} 
          changeType="increase" 
          icon={Ship}
          loading={loading}
        />
        <StatCard 
          title="Active Permits" 
          value={stats.activePermits} 
          change={stats.activePermits > 0 ? "Valid permits" : "No active permits"} 
          changeType={stats.activePermits > 0 ? "increase" : "decrease"} 
          icon={FileText}
          loading={loading}
        />
        <StatCard 
          title="Fishing Operations" 
          value={stats.recentOperations} 
          change={stats.recentOperations > 0 ? "Logged entries" : "No entries"} 
          changeType="increase" 
          icon={Anchor}
          loading={loading}
        />
      </div>

      {/* Recent Activities */}
      <div className="mb-8">
        <h2 className="text-xl font-bold text-gray-900 mb-4">Recent Activities</h2>
        <div className="bg-white border border-gray-200 rounded-xl overflow-hidden shadow-sm">
          <div className="overflow-x-auto">
            <table className="min-w-full divide-y divide-gray-200">
              <thead className="bg-gray-50">
                <tr>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Vessel Name</th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Activity</th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Date</th>
                  <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Status</th>
                </tr>
              </thead>
              <tbody className="bg-white divide-y divide-gray-200">
                {loading ? (
                  <tr>
                    <td colSpan={4} className="px-6 py-8 text-center text-gray-500">
                      <Loader2 className="w-6 h-6 animate-spin mx-auto mb-2" />
                      Loading activities...
                    </td>
                  </tr>
                ) : recentActivities.length === 0 ? (
                  <tr>
                    <td colSpan={4} className="px-6 py-8 text-center text-gray-500">
                      No recent activities found
                    </td>
                  </tr>
                ) : (
                  recentActivities.map((activity) => (
                    <tr key={activity.id} className="hover:bg-gray-50 transition-colors">
                      <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{activity.vesselName}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{activity.type}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{activity.date}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-right">
                        <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${activity.statusColor}`}>
                          {activity.status}
                        </span>
                      </td>
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </div>
        </div>
      </div>

      {/* Promo Card */}
      <div className="relative bg-blue-600 rounded-2xl p-8 overflow-hidden text-white shadow-lg">
        <div className="relative z-10 max-w-2xl">
          <h2 className="text-2xl font-bold mb-2">Need to manage your fleet?</h2>
          <p className="text-blue-100 mb-6">Easily add new vessels, update details, or manage permits from one place.</p>
          <div className="flex space-x-4">
            <Link to="/vessels" className="bg-white text-blue-600 hover:bg-blue-50 px-6 py-2.5 rounded-lg font-medium transition-colors">
              Manage Vessels
            </Link>
            <Link to="/permits" className="bg-blue-500 bg-opacity-30 hover:bg-opacity-40 text-white border border-white border-opacity-30 px-6 py-2.5 rounded-lg font-medium transition-colors">
              Apply for Permit
            </Link>
          </div>
        </div>
        <img 
          src="https://picsum.photos/600/400?random=1" 
          alt="Fisherman" 
          className="absolute bottom-0 right-0 w-1/3 h-full object-cover object-center opacity-90 mask-image-gradient"
          style={{ clipPath: 'polygon(20% 0, 100% 0, 100% 100%, 0% 100%)' }}
        />
      </div>
    </Layout>
  );
};