import React, { useState, useEffect } from 'react';
import { Layout } from '../components/Layout';
import { fishingShipsApi, CreateFishingShipRequest } from '../services/api';

export const Vessels: React.FC = () => {
  const [formData, setFormData] = useState<CreateFishingShipRequest>({
    iaraIdNumber: '',
    maritimeNumber: '',
    shipName: '',
    ownerName: '',
    tonnage: 0,
    shipLength: 0,
    enginePower: 0,
    fuelType: '',
    registrationDate: new Date().toISOString().split('T')[0],
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: ['tonnage', 'shipLength', 'enginePower'].includes(name) 
        ? parseFloat(value) || 0 
        : value
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setSuccess('');
    setLoading(true);

    try {
      await fishingShipsApi.create(formData);
      setSuccess('Vessel registered successfully!');
      
      // Reset form
      setFormData({
        iaraIdNumber: '',
        maritimeNumber: '',
        shipName: '',
        ownerName: '',
        tonnage: 0,
        shipLength: 0,
        enginePower: 0,
        fuelType: '',
        registrationDate: new Date().toISOString().split('T')[0],
      });
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to register vessel. Please try again.');
    } finally {
      setLoading(false);
    }
  };
  return (
    <Layout>
      <div className="flex flex-col xl:flex-row gap-8">
        <div className="flex-1">
          <h1 className="text-4xl font-bold text-gray-900 mb-4">Register Your Vessel</h1>
          <p className="text-gray-500 text-lg mb-8">Let's get your vessel ready for the open waters. Fill in the details to join the IARA fleet!</p>
          
          <div className="rounded-2xl overflow-hidden shadow-lg mb-6">
            <img 
              src="https://picsum.photos/800/600?random=2" 
              alt="Fishing Boat Art" 
              className="w-full h-auto object-cover"
            />
          </div>
        </div>

        <div className="flex-1 bg-white p-8 rounded-2xl shadow-sm border border-gray-100">
          {error && (
            <div className="mb-4 bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg">
              {error}
            </div>
          )}
          {success && (
            <div className="mb-4 bg-green-50 border border-green-200 text-green-700 px-4 py-3 rounded-lg">
              {success}
            </div>
          )}
          
          <form onSubmit={handleSubmit} className="space-y-6">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">IARA ID Number</label>
              <input 
                type="text" 
                name="iaraIdNumber"
                value={formData.iaraIdNumber}
                onChange={handleChange}
                required
                placeholder="e.g., IARA-12345" 
                className="w-full px-4 py-3 rounded-lg border border-gray-300 focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none transition-all" 
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Maritime Number</label>
              <input 
                type="text" 
                name="maritimeNumber"
                value={formData.maritimeNumber}
                onChange={handleChange}
                required
                placeholder="e.g., BG-12345" 
                className="w-full px-4 py-3 rounded-lg border border-gray-300 focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none transition-all" 
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Vessel Name</label>
              <input 
                type="text" 
                name="shipName"
                value={formData.shipName}
                onChange={handleChange}
                required
                placeholder="e.g., Sea Breeze" 
                className="w-full px-4 py-3 rounded-lg border border-gray-300 focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none transition-all" 
              />
            </div>

            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Owner Name</label>
              <input 
                type="text" 
                name="ownerName"
                value={formData.ownerName}
                onChange={handleChange}
                required
                placeholder="e.g., Ivan Petrov" 
                className="w-full px-4 py-3 rounded-lg border border-gray-300 focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none transition-all" 
              />
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Tonnage (GT)</label>
                <input 
                  type="number" 
                  name="tonnage"
                  value={formData.tonnage || ''}
                  onChange={handleChange}
                  required
                  step="0.01"
                  placeholder="e.g., 25" 
                  className="w-full px-4 py-3 rounded-lg border border-gray-300 focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none transition-all" 
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Ship Length (m)</label>
                <input 
                  type="number" 
                  name="shipLength"
                  value={formData.shipLength || ''}
                  onChange={handleChange}
                  required
                  step="0.01"
                  placeholder="e.g., 15.5" 
                  className="w-full px-4 py-3 rounded-lg border border-gray-300 focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none transition-all" 
                />
              </div>
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Engine Power (kW)</label>
                <input 
                  type="number" 
                  name="enginePower"
                  value={formData.enginePower || ''}
                  onChange={handleChange}
                  required
                  step="0.01"
                  placeholder="e.g., 150" 
                  className="w-full px-4 py-3 rounded-lg border border-gray-300 focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none transition-all" 
                />
              </div>
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-1">Fuel Type</label>
                <input 
                  type="text" 
                  name="fuelType"
                  value={formData.fuelType}
                  onChange={handleChange}
                  placeholder="e.g., Diesel" 
                  className="w-full px-4 py-3 rounded-lg border border-gray-300 focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none transition-all" 
                />
              </div>
            </div>

             <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Registration Date</label>
              <input 
                type="date" 
                name="registrationDate"
                value={formData.registrationDate}
                onChange={handleChange}
                required
                className="w-full px-4 py-3 rounded-lg border border-gray-300 focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none transition-all" 
              />
            </div>

            <button 
              type="submit"
              disabled={loading}
              className="w-full bg-blue-600 hover:bg-blue-700 text-white font-bold py-4 rounded-xl transition-colors shadow-lg shadow-blue-200 mt-4 disabled:opacity-50 disabled:cursor-not-allowed"
            >
              {loading ? 'Registering...' : 'Register Vessel'}
            </button>
          </form>
        </div>
      </div>
    </Layout>
  );
};