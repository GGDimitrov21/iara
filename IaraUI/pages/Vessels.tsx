import React, { useState, useEffect } from 'react';
import { Layout } from '../components/Layout';
import { vesselsApi, CreateVesselRequest, VesselDto } from '../services/api';
import { Ship, Plus, Trash2, AlertCircle } from 'lucide-react';

export const Vessels: React.FC = () => {
  const [vessels, setVessels] = useState<VesselDto[]>([]);
  const [showForm, setShowForm] = useState(false);
  const [loadingVessels, setLoadingVessels] = useState(true);
  const [loadError, setLoadError] = useState<string | null>(null);
  const [formData, setFormData] = useState<CreateVesselRequest>({
    regNumber: '',
    vesselName: '',
    ownerDetails: '',
    tonnage: undefined,
    lengthM: undefined,
    widthM: undefined,
    enginePowerKw: undefined,
    fuelType: '',
    displacementTons: undefined,
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  useEffect(() => {
    loadVessels();
  }, []);

  const loadVessels = async () => {
    try {
      setLoadingVessels(true);
      const data = await vesselsApi.getAll();
      setVessels(data);
      setLoadError(null);
    } catch (err) {
      setLoadError('Failed to load vessels');
      console.error(err);
    } finally {
      setLoadingVessels(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: ['tonnage', 'lengthM', 'widthM', 'enginePowerKw', 'displacementTons'].includes(name) 
        ? (value === '' ? undefined : parseFloat(value))
        : value
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setSuccess('');
    setLoading(true);

    try {
      await vesselsApi.create(formData);
      setSuccess('Vessel registered successfully!');
      loadVessels();
      setShowForm(false);
      
      // Reset form
      setFormData({
        regNumber: '',
        vesselName: '',
        ownerDetails: '',
        tonnage: undefined,
        lengthM: undefined,
        widthM: undefined,
        enginePowerKw: undefined,
        fuelType: '',
        displacementTons: undefined,
      });
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to register vessel. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (vesselId: number) => {
    if (!confirm('Are you sure you want to delete this vessel?')) return;
    try {
      await vesselsApi.delete(vesselId);
      loadVessels();
    } catch (err) {
      setLoadError('Failed to delete vessel');
    }
  };

  return (
    <Layout>
      <div className="flex flex-col md:flex-row justify-between items-start md:items-center mb-8 gap-4">
        <div>
          <h1 className="text-3xl font-bold text-gray-900">Vessels Management</h1>
          <p className="text-gray-500 mt-1">View and manage all registered fishing vessels.</p>
        </div>
        <button 
          onClick={() => setShowForm(!showForm)}
          className="bg-green-400 hover:bg-green-500 text-gray-900 font-bold px-6 py-2.5 rounded-full flex items-center shadow-sm transition-colors"
        >
          <Plus className="w-5 h-5 mr-2" />
          {showForm ? 'Hide Form' : 'Register New Vessel'}
        </button>
      </div>

      {loadError && (
        <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg mb-6 flex items-center gap-2">
          <AlertCircle className="w-5 h-5" />
          {loadError}
        </div>
      )}

      {success && (
        <div className="mb-4 bg-green-50 border border-green-200 text-green-700 px-4 py-3 rounded-lg">
          {success}
        </div>
      )}

      {showForm && (
        <div className="bg-white p-6 rounded-2xl shadow-sm border border-gray-100 mb-8">
          <h2 className="text-xl font-semibold mb-4">Register New Vessel</h2>
          {error && (
            <div className="mb-4 bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg">
              {error}
            </div>
          )}
          
          <form onSubmit={handleSubmit} className="space-y-4">
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Registration Number *</label>
                <input 
                  type="text" 
                  name="regNumber"
                  value={formData.regNumber}
                  onChange={handleChange}
                  required
                  placeholder="e.g., BG-VAR-12345" 
                  className="w-full px-4 py-3 rounded-lg border border-gray-300 focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none transition-all" 
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Vessel Name *</label>
                <input 
                  type="text" 
                  name="vesselName"
                  value={formData.vesselName}
                  onChange={handleChange}
                  required
                  placeholder="e.g., Sea Breeze" 
                  className="w-full px-4 py-3 rounded-lg border border-gray-300 focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none transition-all" 
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Owner Details</label>
                <input 
                  type="text" 
                  name="ownerDetails"
                  value={formData.ownerDetails || ''}
                  onChange={handleChange}
                  placeholder="e.g., Ivan Petrov" 
                  className="w-full px-4 py-3 rounded-lg border border-gray-300 focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none transition-all" 
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Length (m)</label>
                <input 
                  type="number" 
                  name="lengthM"
                  value={formData.lengthM ?? ''}
                  onChange={handleChange}
                  step="0.01"
                  placeholder="e.g., 15.5" 
                  className="w-full px-4 py-3 rounded-lg border border-gray-300 focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none transition-all" 
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Width (m)</label>
                <input 
                  type="number" 
                  name="widthM"
                  value={formData.widthM ?? ''}
                  onChange={handleChange}
                  step="0.01"
                  placeholder="e.g., 5.2" 
                  className="w-full px-4 py-3 rounded-lg border border-gray-300 focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none transition-all" 
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Tonnage</label>
                <input 
                  type="number" 
                  name="tonnage"
                  value={formData.tonnage ?? ''}
                  onChange={handleChange}
                  step="0.01"
                  placeholder="e.g., 25" 
                  className="w-full px-4 py-3 rounded-lg border border-gray-300 focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none transition-all" 
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Engine Power (kW)</label>
                <input 
                  type="number" 
                  name="enginePowerKw"
                  value={formData.enginePowerKw ?? ''}
                  onChange={handleChange}
                  placeholder="e.g., 150" 
                  className="w-full px-4 py-3 rounded-lg border border-gray-300 focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none transition-all" 
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Fuel Type</label>
                <input 
                  type="text" 
                  name="fuelType"
                  value={formData.fuelType || ''}
                  onChange={handleChange}
                  placeholder="e.g., Diesel" 
                  className="w-full px-4 py-3 rounded-lg border border-gray-300 focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none transition-all" 
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Displacement (tons)</label>
                <input 
                  type="number" 
                  name="displacementTons"
                  value={formData.displacementTons ?? ''}
                  onChange={handleChange}
                  step="0.01"
                  placeholder="e.g., 30" 
                  className="w-full px-4 py-3 rounded-lg border border-gray-300 focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none transition-all" 
                />
              </div>
            </div>

            <button 
              type="submit"
              disabled={loading}
              className="bg-blue-600 hover:bg-blue-700 text-white font-bold py-3 px-8 rounded-xl transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {loading ? 'Registering...' : 'Register Vessel'}
            </button>
          </form>
        </div>
      )}

      {/* Vessels Table */}
      <div className="bg-white rounded-2xl shadow-sm border border-gray-100 overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full">
            <thead className="bg-gray-50 border-b border-gray-100">
              <tr>
                <th className="px-6 py-4 text-left text-sm font-semibold text-blue-600">REG. NUMBER</th>
                <th className="px-6 py-4 text-left text-sm font-semibold text-blue-600">VESSEL NAME</th>
                <th className="px-6 py-4 text-left text-sm font-semibold text-blue-600">OWNER</th>
                <th className="px-6 py-4 text-left text-sm font-semibold text-blue-600">CAPTAIN</th>
                <th className="px-6 py-4 text-left text-sm font-semibold text-blue-600">LENGTH (M)</th>
                <th className="px-6 py-4 text-left text-sm font-semibold text-blue-600">TONNAGE</th>
                <th className="px-6 py-4 text-left text-sm font-semibold text-blue-600">ENGINE (KW)</th>
                <th className="px-6 py-4 text-left text-sm font-semibold text-blue-600">ACTIONS</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-100">
              {loadingVessels ? (
                <tr>
                  <td colSpan={8} className="px-6 py-8 text-center text-gray-500">Loading vessels...</td>
                </tr>
              ) : vessels.length === 0 ? (
                <tr>
                  <td colSpan={8} className="px-6 py-8 text-center text-gray-500">
                    <Ship className="w-12 h-12 mx-auto mb-2 text-gray-300" />
                    No vessels found. Register your first vessel above.
                  </td>
                </tr>
              ) : (
                vessels.map(vessel => (
                  <tr key={vessel.vesselId} className="hover:bg-gray-50">
                    <td className="px-6 py-4 font-medium text-gray-900">{vessel.regNumber}</td>
                    <td className="px-6 py-4 text-gray-700">{vessel.vesselName}</td>
                    <td className="px-6 py-4 text-gray-600">{vessel.ownerDetails || '-'}</td>
                    <td className="px-6 py-4 text-gray-600">{vessel.captainName || '-'}</td>
                    <td className="px-6 py-4 text-gray-600">{vessel.lengthM?.toFixed(1) || '-'}</td>
                    <td className="px-6 py-4 text-gray-600">{vessel.tonnage?.toFixed(1) || '-'}</td>
                    <td className="px-6 py-4 text-gray-600">{vessel.enginePowerKw?.toFixed(0) || '-'}</td>
                    <td className="px-6 py-4">
                      <button
                        onClick={() => handleDelete(vessel.vesselId)}
                        className="text-red-500 hover:text-red-700 p-1"
                        title="Delete vessel"
                      >
                        <Trash2 className="w-5 h-5" />
                      </button>
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </div>
    </Layout>
  );
};