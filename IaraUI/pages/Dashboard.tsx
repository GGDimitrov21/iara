import React from 'react';
import { Layout } from '../components/Layout';
import { Ship, FileText, Anchor, ArrowUpRight, ArrowDownRight } from 'lucide-react';
import { Link } from 'react-router-dom';

const StatCard = ({ title, value, change, changeType, icon: Icon }: any) => (
  <div className="bg-white p-6 rounded-xl border border-gray-100 shadow-sm relative overflow-hidden">
    <div className="flex justify-between items-start z-10 relative">
      <div>
        <p className="text-sm font-medium text-gray-500">{title}</p>
        <h3 className="text-3xl font-bold text-gray-900 mt-2">{value}</h3>
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

export const Dashboard: React.FC = () => {
  const recentActivities = [
    { vessel: "Vessel 'Neptune'", permit: 'Trawling', date: '2024-07-20', status: 'Completed', statusColor: 'bg-green-100 text-green-800' },
    { vessel: "Vessel 'Poseidon'", permit: 'Gillnetting', date: '2024-07-22', status: 'In Progress', statusColor: 'bg-blue-100 text-blue-800' },
    { vessel: "Vessel 'Triton'", permit: 'Longlining', date: '2024-07-25', status: 'Scheduled', statusColor: 'bg-yellow-100 text-yellow-800' },
    { vessel: "Vessel 'Oceanus'", permit: 'Potting', date: '2024-07-28', status: 'Completed', statusColor: 'bg-green-100 text-green-800' },
    { vessel: "Vessel 'Proteus'", permit: 'Seining', date: '2024-07-30', status: 'Scheduled', statusColor: 'bg-yellow-100 text-yellow-800' },
  ];

  return (
    <Layout>
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-gray-900">Dashboard</h1>
        <p className="text-gray-500 mt-2">Welcome back, Personnel! Here's an overview of your fishing activities.</p>
      </div>

      {/* Stats Grid */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
        <StatCard 
          title="Total Vessels" 
          value="12" 
          change="+10%" 
          changeType="increase" 
          icon={Ship} 
        />
        <StatCard 
          title="Active Permits" 
          value="8" 
          change="-5%" 
          changeType="decrease" 
          icon={FileText} 
        />
        <StatCard 
          title="Recent Operations" 
          value="25" 
          change="+15%" 
          changeType="increase" 
          icon={Anchor} 
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
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Permit Type</th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Operation Date</th>
                  <th className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider">Status</th>
                </tr>
              </thead>
              <tbody className="bg-white divide-y divide-gray-200">
                {recentActivities.map((activity, idx) => (
                  <tr key={idx} className="hover:bg-gray-50 transition-colors">
                    <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{activity.vessel}</td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{activity.permit}</td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{activity.date}</td>
                    <td className="px-6 py-4 whitespace-nowrap text-right">
                      <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${activity.statusColor}`}>
                        {activity.status}
                      </span>
                    </td>
                  </tr>
                ))}
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