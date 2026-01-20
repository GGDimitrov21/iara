import React, { useState, useEffect } from 'react';
import { Layout } from '../components/Layout';
import { Plus, Search, Calendar, Edit2, Trash2, ChevronLeft, ChevronRight, X, AlertCircle } from 'lucide-react';
import { CatchQuota, Permit, Species } from '../types';
import { quotasApi, permitsApi, speciesApi, CreateCatchQuotaRequest } from '../services/api';

export const Quotas: React.FC = () => {
  const [quotas, setQuotas] = useState<CatchQuota[]>([]);
  const [permits, setPermits] = useState<Permit[]>([]);
  const [speciesList, setSpeciesList] = useState<Species[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showModal, setShowModal] = useState(false);
  const [formData, setFormData] = useState<CreateCatchQuotaRequest>({
    permitId: 0,
    speciesId: 0,
    year: new Date().getFullYear(),
    maxCatchKg: 0,
    avgCatchKg: 0,
    minCatchKg: 0,
    fuelHoursLimit: 0
  });

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      setLoading(true);
      const [quotasData, permitsData, speciesData] = await Promise.all([
        quotasApi.getAll(),
        permitsApi.getAll(),
        speciesApi.getAll()
      ]);
      setQuotas(quotasData);
      setPermits(permitsData);
      setSpeciesList(speciesData);
      setError(null);
    } catch (err) {
      setError('Failed to load data');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await quotasApi.create(formData);
      setShowModal(false);
      loadData();
      setFormData({
        permitId: 0, speciesId: 0, year: new Date().getFullYear(),
        maxCatchKg: 0, avgCatchKg: 0, minCatchKg: 0, fuelHoursLimit: 0
      });
    } catch (err) {
      setError('Failed to create quota');
    }
  };

  const handleDelete = async (quotaId: number) => {
    if (!confirm('Are you sure you want to delete this quota?')) return;
    try {
      await quotasApi.delete(quotaId);
      loadData();
    } catch (err) {
      setError('Failed to delete quota');
    }
  };

  const getSpeciesName = (speciesId: number) => {
    const species = speciesList.find(s => s.speciesId === speciesId);
    return species?.speciesName || `Species #${speciesId}`;
  };

  return (
    <Layout>
      <div className="flex flex-col md:flex-row justify-between items-start md:items-center mb-8 gap-4">
        <div>
          <h1 className="text-3xl font-bold text-gray-900">Catch Quotas Management</h1>
          <p className="text-gray-500 mt-1">View, add, and edit catch quotas for all permits.</p>
        </div>
        <button 
          onClick={() => setShowModal(true)}
          className="bg-green-400 hover:bg-green-500 text-gray-900 font-bold px-6 py-2.5 rounded-full flex items-center shadow-sm transition-colors"
        >
          <Plus className="w-5 h-5 mr-2" />
          Add New Quota
        </button>
      </div>

      {error && (
        <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg mb-6 flex items-center gap-2">
          <AlertCircle className="w-5 h-5" />
          {error}
        </div>
      )}

      {/* Filters */}
      <div className="grid grid-cols-1 md:grid-cols-12 gap-4 mb-6">
        <div className="md:col-span-5 relative">
          <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
            <Search className="h-5 w-5 text-gray-400" />
          </div>
          <input 
            type="text" 
            placeholder="Search by Species Name..." 
            className="block w-full pl-10 pr-3 py-3 border border-gray-200 rounded-full leading-5 bg-white placeholder-gray-500 focus:outline-none focus:ring-1 focus:ring-green-400 focus:border-green-400 sm:text-sm shadow-sm"
          />
        </div>
        
        <div className="md:col-span-3 relative">
           <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
            <Calendar className="h-5 w-5 text-gray-400" />
          </div>
          <input 
            type="text" 
            placeholder="Year..." 
            className="block w-full pl-10 pr-3 py-3 border border-gray-200 rounded-full leading-5 bg-white placeholder-gray-500 focus:outline-none focus:ring-1 focus:ring-green-400 focus:border-green-400 sm:text-sm shadow-sm"
          />
        </div>

        <div className="md:col-span-2">
           <select className="block w-full pl-4 pr-10 py-3 text-base border border-gray-200 focus:outline-none focus:ring-green-400 focus:border-green-400 sm:text-sm rounded-full shadow-sm bg-white">
             <option>Permit ID: All</option>
           </select>
        </div>
         <div className="md:col-span-2">
           <select className="block w-full pl-4 pr-10 py-3 text-base border border-gray-200 focus:outline-none focus:ring-green-400 focus:border-green-400 sm:text-sm rounded-full shadow-sm bg-white">
             <option>Status: All</option>
           </select>
        </div>
      </div>

      {/* Table */}
      <div className="bg-white rounded-3xl shadow-sm border border-gray-100 overflow-hidden">
        <div className="overflow-x-auto">
          <table className="min-w-full divide-y divide-gray-100">
            <thead>
              <tr className="bg-gray-50 bg-opacity-50">
                <th className="px-6 py-5 text-left text-xs font-bold text-gray-500 uppercase tracking-wider">Permit ID</th>
                <th className="px-6 py-5 text-left text-xs font-bold text-gray-500 uppercase tracking-wider">Species Name</th>
                <th className="px-6 py-5 text-left text-xs font-bold text-gray-500 uppercase tracking-wider">Year</th>
                <th className="px-6 py-5 text-center text-xs font-bold text-gray-500 uppercase tracking-wider">Min Catch (KG)</th>
                <th className="px-6 py-5 text-center text-xs font-bold text-gray-500 uppercase tracking-wider">Avg Catch (KG)</th>
                <th className="px-6 py-5 text-center text-xs font-bold text-gray-500 uppercase tracking-wider">Max Catch (KG)</th>
                <th className="px-6 py-5 text-center text-xs font-bold text-gray-500 uppercase tracking-wider">Fuel Hours Limit</th>
                <th className="px-6 py-5 text-center text-xs font-bold text-gray-500 uppercase tracking-wider">Actions</th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-100">
              {loading ? (
                <tr><td colSpan={8} className="px-6 py-8 text-center text-gray-500">Loading...</td></tr>
              ) : quotas.length === 0 ? (
                <tr><td colSpan={8} className="px-6 py-8 text-center text-gray-500">No quotas found</td></tr>
              ) : quotas.map((quota) => (
                <tr key={quota.quotaId} className="hover:bg-gray-50 transition-colors">
                  <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{quota.permitId}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-600">{getSpeciesName(quota.speciesId)}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-600">{quota.year}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-600 text-center">{quota.minCatchKg?.toLocaleString() || '-'}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-600 text-center">{quota.avgCatchKg?.toLocaleString() || '-'}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-600 text-center">{quota.maxCatchKg?.toLocaleString() || '-'}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-600 text-center">{quota.fuelHoursLimit || '-'}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-center text-sm font-medium">
                    <div className="flex justify-center space-x-3">
                      <button className="text-gray-400 hover:text-blue-600 transition-colors">
                        <Edit2 className="w-5 h-5" />
                      </button>
                      <button 
                        onClick={() => handleDelete(quota.quotaId)}
                        className="text-gray-400 hover:text-red-600 transition-colors"
                      >
                        <Trash2 className="w-5 h-5" />
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
      
      <div className="mt-4 flex items-center justify-between">
         <p className="text-sm text-gray-500">Showing <span className="font-bold text-gray-900">1</span> to <span className="font-bold text-gray-900">{quotas.length}</span> of <span className="font-bold text-gray-900">{quotas.length}</span> results</p>
         <div className="flex space-x-2">
            <button className="p-2 rounded-full border border-gray-200 bg-white hover:bg-gray-50 text-gray-500"><ChevronLeft className="w-5 h-5" /></button>
            <button className="p-2 rounded-full border border-gray-200 bg-white hover:bg-gray-50 text-gray-500"><ChevronRight className="w-5 h-5" /></button>
         </div>
      </div>

      {showModal && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
          <div className="bg-white rounded-xl p-6 w-full max-w-lg">
            <div className="flex justify-between items-center mb-4">
              <h2 className="text-xl font-bold">Add New Quota</h2>
              <button onClick={() => setShowModal(false)}>
                <X className="w-5 h-5 text-gray-500" />
              </button>
            </div>
            <form onSubmit={handleSubmit}>
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Permit</label>
                  <select
                    value={formData.permitId}
                    onChange={(e) => setFormData({ ...formData, permitId: parseInt(e.target.value) })}
                    className="w-full border border-gray-300 rounded-lg px-3 py-2"
                    required
                  >
                    <option value={0}>Select a permit</option>
                    {permits.map(permit => (
                      <option key={permit.permitId} value={permit.permitId}>Permit #{permit.permitId}</option>
                    ))}
                  </select>
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Species</label>
                  <select
                    value={formData.speciesId}
                    onChange={(e) => setFormData({ ...formData, speciesId: parseInt(e.target.value) })}
                    className="w-full border border-gray-300 rounded-lg px-3 py-2"
                    required
                  >
                    <option value={0}>Select species</option>
                    {speciesList.map(species => (
                      <option key={species.speciesId} value={species.speciesId}>{species.speciesName}</option>
                    ))}
                  </select>
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Year</label>
                  <input
                    type="number"
                    value={formData.year}
                    onChange={(e) => setFormData({ ...formData, year: parseInt(e.target.value) })}
                    className="w-full border border-gray-300 rounded-lg px-3 py-2"
                    required
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Min Catch (KG)</label>
                  <input
                    type="number"
                    value={formData.minCatchKg}
                    onChange={(e) => setFormData({ ...formData, minCatchKg: parseFloat(e.target.value) })}
                    className="w-full border border-gray-300 rounded-lg px-3 py-2"
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Avg Catch (KG)</label>
                  <input
                    type="number"
                    value={formData.avgCatchKg}
                    onChange={(e) => setFormData({ ...formData, avgCatchKg: parseFloat(e.target.value) })}
                    className="w-full border border-gray-300 rounded-lg px-3 py-2"
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Max Catch (KG)</label>
                  <input
                    type="number"
                    value={formData.maxCatchKg}
                    onChange={(e) => setFormData({ ...formData, maxCatchKg: parseFloat(e.target.value) })}
                    className="w-full border border-gray-300 rounded-lg px-3 py-2"
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Fuel Hours Limit</label>
                  <input
                    type="number"
                    value={formData.fuelHoursLimit}
                    onChange={(e) => setFormData({ ...formData, fuelHoursLimit: parseInt(e.target.value) })}
                    className="w-full border border-gray-300 rounded-lg px-3 py-2"
                  />
                </div>
              </div>
              <div className="flex gap-3 mt-6">
                <button type="button" onClick={() => setShowModal(false)} className="flex-1 px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50">
                  Cancel
                </button>
                <button type="submit" className="flex-1 px-4 py-2 bg-green-500 text-white rounded-lg hover:bg-green-600">
                  Add Quota
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </Layout>
  );
};