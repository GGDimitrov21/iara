import React, { useState, useEffect } from 'react';
import { Layout } from '../components/Layout';
import { Search, Fish, Plus, X, AlertCircle } from 'lucide-react';
import { Permit, Vessel } from '../types';
import { permitsApi, vesselsApi, CreatePermitRequest } from '../services/api';

export const Permits: React.FC = () => {
  const [permits, setPermits] = useState<Permit[]>([]);
  const [vessels, setVessels] = useState<Vessel[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showModal, setShowModal] = useState(false);
  const [formData, setFormData] = useState<CreatePermitRequest>({
    vesselId: 0,
    issueDate: new Date().toISOString().split('T')[0],
    expiryDate: '',
    isActive: true
  });

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      setLoading(true);
      const [permitsData, vesselsData] = await Promise.all([
        permitsApi.getAll(),
        vesselsApi.getAll()
      ]);
      setPermits(permitsData);
      setVessels(vesselsData);
      setError(null);
    } catch (err) {
      setError('Failed to load permits');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await permitsApi.create(formData);
      setShowModal(false);
      loadData();
      setFormData({ vesselId: 0, issueDate: new Date().toISOString().split('T')[0], expiryDate: '', isActive: true });
    } catch (err) {
      setError('Failed to create permit');
    }
  };

  const getVesselName = (vesselId: number) => {
    const vessel = vessels.find(v => v.vesselId === vesselId);
    return vessel?.vesselName || `Vessel #${vesselId}`;
  };

  const getStatus = (permit: Permit): 'Active' | 'Expired' => {
    if (!permit.isActive) return 'Expired';
    return new Date(permit.expiryDate) > new Date() ? 'Active' : 'Expired';
  };

  return (
    <Layout>
      <div className="flex flex-col md:flex-row justify-between items-start md:items-center mb-8 gap-4">
        <div className="flex items-center gap-4">
          <div className="p-3 bg-yellow-100 rounded-xl">
             <Fish className="w-8 h-8 text-yellow-700" />
          </div>
          <div>
            <h1 className="text-3xl font-bold text-gray-900">Permit Management</h1>
            <p className="text-gray-500">Easily issue, view, and manage all your fishing permits.</p>
          </div>
        </div>
        <button 
          onClick={() => setShowModal(true)}
          className="bg-blue-600 hover:bg-blue-700 text-white px-6 py-2.5 rounded-lg font-medium transition-colors shadow-sm flex items-center gap-2"
        >
          <Plus className="w-4 h-4" />
          Issue New Permit
        </button>
      </div>

      {error && (
        <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg mb-6 flex items-center gap-2">
          <AlertCircle className="w-5 h-5" />
          {error}
        </div>
      )}

      <div className="bg-blue-50 p-4 rounded-xl mb-6">
        <div className="relative">
          <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-blue-400 w-5 h-5" />
          <input 
            type="text" 
            placeholder="Search by Permit ID or Vessel Name..." 
            className="w-full pl-10 pr-4 py-3 bg-transparent border-none focus:ring-0 text-blue-900 placeholder-blue-400"
          />
        </div>
      </div>

      <div className="bg-white border border-gray-200 rounded-xl overflow-hidden shadow-sm">
        <div className="overflow-x-auto">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-blue-50">
              <tr>
                <th className="px-6 py-4 text-left text-xs font-semibold text-blue-600 uppercase tracking-wider">Permit ID</th>
                <th className="px-6 py-4 text-left text-xs font-semibold text-blue-600 uppercase tracking-wider">Vessel</th>
                <th className="px-6 py-4 text-left text-xs font-semibold text-blue-600 uppercase tracking-wider">Issue Date</th>
                <th className="px-6 py-4 text-left text-xs font-semibold text-blue-600 uppercase tracking-wider">Expiry Date</th>
                <th className="px-6 py-4 text-center text-xs font-semibold text-blue-600 uppercase tracking-wider">Status</th>
                <th className="px-6 py-4 text-right text-xs font-semibold text-blue-600 uppercase tracking-wider">Actions</th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {loading ? (
                <tr><td colSpan={6} className="px-6 py-8 text-center text-gray-500">Loading...</td></tr>
              ) : permits.length === 0 ? (
                <tr><td colSpan={6} className="px-6 py-8 text-center text-gray-500">No permits found</td></tr>
              ) : permits.map((permit) => {
                const status = getStatus(permit);
                return (
                  <tr key={permit.permitId} className="hover:bg-gray-50 transition-colors">
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{permit.permitId}</td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900 font-medium">{getVesselName(permit.vesselId)}</td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{new Date(permit.issueDate).toLocaleDateString()}</td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{new Date(permit.expiryDate).toLocaleDateString()}</td>
                    <td className="px-6 py-4 whitespace-nowrap text-center">
                      <span className={`inline-flex items-center px-3 py-1 rounded-full text-xs font-medium ${
                        status === 'Active' 
                          ? 'bg-blue-100 text-blue-800' 
                          : 'bg-red-100 text-red-800'
                      }`}>
                        {status}
                      </span>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                      <button className="text-blue-600 hover:text-blue-900 font-semibold">View</button>
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>
      </div>

      {showModal && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
          <div className="bg-white rounded-xl p-6 w-full max-w-md">
            <div className="flex justify-between items-center mb-4">
              <h2 className="text-xl font-bold">Issue New Permit</h2>
              <button onClick={() => setShowModal(false)}>
                <X className="w-5 h-5 text-gray-500" />
              </button>
            </div>
            <form onSubmit={handleSubmit}>
              <div className="space-y-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Vessel</label>
                  <select
                    value={formData.vesselId}
                    onChange={(e) => setFormData({ ...formData, vesselId: parseInt(e.target.value) })}
                    className="w-full border border-gray-300 rounded-lg px-3 py-2"
                    required
                  >
                    <option value={0}>Select a vessel</option>
                    {vessels.map(vessel => (
                      <option key={vessel.vesselId} value={vessel.vesselId}>{vessel.vesselName}</option>
                    ))}
                  </select>
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Issue Date</label>
                  <input
                    type="date"
                    value={formData.issueDate}
                    onChange={(e) => setFormData({ ...formData, issueDate: e.target.value })}
                    className="w-full border border-gray-300 rounded-lg px-3 py-2"
                    required
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Expiry Date</label>
                  <input
                    type="date"
                    value={formData.expiryDate}
                    onChange={(e) => setFormData({ ...formData, expiryDate: e.target.value })}
                    className="w-full border border-gray-300 rounded-lg px-3 py-2"
                    required
                  />
                </div>
                <div className="flex items-center gap-2">
                  <input
                    type="checkbox"
                    id="isActive"
                    checked={formData.isActive}
                    onChange={(e) => setFormData({ ...formData, isActive: e.target.checked })}
                    className="rounded border-gray-300"
                  />
                  <label htmlFor="isActive" className="text-sm text-gray-700">Active</label>
                </div>
              </div>
              <div className="flex gap-3 mt-6">
                <button type="button" onClick={() => setShowModal(false)} className="flex-1 px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50">
                  Cancel
                </button>
                <button type="submit" className="flex-1 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700">
                  Issue Permit
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </Layout>
  );
};