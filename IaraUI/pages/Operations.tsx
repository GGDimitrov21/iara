import React, { useState, useEffect } from 'react';
import { Layout } from '../components/Layout';
import { Calendar, AlertCircle, Plus, X } from 'lucide-react';
import { LogbookEntry, Vessel, Personnel, Species } from '../types';
import { logbookApi, vesselsApi, personnelApi, speciesApi, CreateLogbookEntryRequest } from '../services/api';

export const Operations: React.FC = () => {
  const [logEntries, setLogEntries] = useState<LogbookEntry[]>([]);
  const [vessels, setVessels] = useState<Vessel[]>([]);
  const [captains, setCaptains] = useState<Personnel[]>([]);
  const [speciesList, setSpeciesList] = useState<Species[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showModal, setShowModal] = useState(false);
  const [formData, setFormData] = useState<CreateLogbookEntryRequest>({
    vesselId: 0,
    captainId: 0,
    startTime: new Date().toISOString(),
    durationHours: 0,
    latitude: 0,
    longitude: 0,
    speciesId: 0,
    catchKg: 0
  });

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      setLoading(true);
      const [logData, vesselsData, personnelData, speciesData] = await Promise.all([
        logbookApi.getAll(),
        vesselsApi.getAll(),
        personnelApi.getByRole('Captain'),
        speciesApi.getAll()
      ]);
      setLogEntries(logData);
      setVessels(vesselsData);
      setCaptains(personnelData);
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
      await logbookApi.create(formData);
      setShowModal(false);
      loadData();
      setFormData({
        vesselId: 0, captainId: 0, startTime: new Date().toISOString(),
        durationHours: 0, latitude: 0, longitude: 0, speciesId: 0, catchKg: 0
      });
    } catch (err) {
      setError('Failed to create log entry');
    }
  };

  const getVesselName = (vesselId: number) => vessels.find(v => v.vesselId === vesselId)?.vesselName || `Vessel #${vesselId}`;
  const getSpeciesName = (speciesId: number) => speciesList.find(s => s.speciesId === speciesId)?.speciesName || `Species #${speciesId}`;

  return (
    <Layout>
      <div className="mb-8 flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-bold text-gray-900 text-blue-900">Fishing Operations Log</h1>
          <p className="text-gray-500">Record your daily fishing activities with precision and detail.</p>
        </div>
        <button 
          onClick={() => setShowModal(true)}
          className="bg-blue-700 hover:bg-blue-800 text-white px-6 py-2.5 rounded-lg font-medium flex items-center gap-2"
        >
          <Plus className="w-4 h-4" />
          New Log Entry
        </button>
      </div>

      {error && (
        <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg mb-6 flex items-center gap-2">
          <AlertCircle className="w-5 h-5" />
          {error}
        </div>
      )}

      <div className="bg-white rounded-xl shadow-sm border border-gray-100 overflow-hidden">
        <div className="overflow-x-auto">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-blue-50">
              <tr>
                <th className="px-6 py-4 text-left text-xs font-semibold text-blue-600 uppercase tracking-wider">Date</th>
                <th className="px-6 py-4 text-left text-xs font-semibold text-blue-600 uppercase tracking-wider">Vessel</th>
                <th className="px-6 py-4 text-left text-xs font-semibold text-blue-600 uppercase tracking-wider">Species</th>
                <th className="px-6 py-4 text-left text-xs font-semibold text-blue-600 uppercase tracking-wider">Catch (KG)</th>
                <th className="px-6 py-4 text-left text-xs font-semibold text-blue-600 uppercase tracking-wider">Duration (hrs)</th>
                <th className="px-6 py-4 text-left text-xs font-semibold text-blue-600 uppercase tracking-wider">Location</th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-200">
              {loading ? (
                <tr><td colSpan={6} className="px-6 py-8 text-center text-gray-500">Loading...</td></tr>
              ) : logEntries.length === 0 ? (
                <tr><td colSpan={6} className="px-6 py-8 text-center text-gray-500">No log entries found</td></tr>
              ) : logEntries.map((entry) => (
                <tr key={entry.logEntryId} className="hover:bg-gray-50">
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-700">{new Date(entry.startTime).toLocaleDateString()}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-700">{getVesselName(entry.vesselId)}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-700">{getSpeciesName(entry.speciesId)}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-700">{entry.catchKg || '-'}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-700">{entry.durationHours || '-'}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-700">
                    {entry.latitude && entry.longitude ? `${entry.latitude.toFixed(4)}, ${entry.longitude.toFixed(4)}` : '-'}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>

      {showModal && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
          <div className="bg-white rounded-xl p-6 w-full max-w-lg">
            <div className="flex justify-between items-center mb-4">
              <h2 className="text-xl font-bold">New Log Entry</h2>
              <button onClick={() => setShowModal(false)}>
                <X className="w-5 h-5 text-gray-500" />
              </button>
            </div>
            <form onSubmit={handleSubmit}>
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Vessel</label>
                  <select
                    value={formData.vesselId}
                    onChange={(e) => setFormData({ ...formData, vesselId: parseInt(e.target.value) })}
                    className="w-full border border-gray-300 rounded-lg px-3 py-2"
                    required
                  >
                    <option value={0}>Select vessel</option>
                    {vessels.map(v => <option key={v.vesselId} value={v.vesselId}>{v.vesselName}</option>)}
                  </select>
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Captain</label>
                  <select
                    value={formData.captainId}
                    onChange={(e) => setFormData({ ...formData, captainId: parseInt(e.target.value) })}
                    className="w-full border border-gray-300 rounded-lg px-3 py-2"
                    required
                  >
                    <option value={0}>Select captain</option>
                    {captains.map(c => <option key={c.personId} value={c.personId}>{c.name}</option>)}
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
                    {speciesList.map(s => <option key={s.speciesId} value={s.speciesId}>{s.speciesName}</option>)}
                  </select>
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Catch (kg)</label>
                  <input
                    type="number"
                    value={formData.catchKg}
                    onChange={(e) => setFormData({ ...formData, catchKg: parseFloat(e.target.value) })}
                    className="w-full border border-gray-300 rounded-lg px-3 py-2"
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Duration (hours)</label>
                  <input
                    type="number"
                    value={formData.durationHours}
                    onChange={(e) => setFormData({ ...formData, durationHours: parseInt(e.target.value) })}
                    className="w-full border border-gray-300 rounded-lg px-3 py-2"
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Start Time</label>
                  <input
                    type="datetime-local"
                    value={formData.startTime.slice(0, 16)}
                    onChange={(e) => setFormData({ ...formData, startTime: new Date(e.target.value).toISOString() })}
                    className="w-full border border-gray-300 rounded-lg px-3 py-2"
                    required
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Latitude</label>
                  <input
                    type="number"
                    step="0.0001"
                    value={formData.latitude}
                    onChange={(e) => setFormData({ ...formData, latitude: parseFloat(e.target.value) })}
                    className="w-full border border-gray-300 rounded-lg px-3 py-2"
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Longitude</label>
                  <input
                    type="number"
                    step="0.0001"
                    value={formData.longitude}
                    onChange={(e) => setFormData({ ...formData, longitude: parseFloat(e.target.value) })}
                    className="w-full border border-gray-300 rounded-lg px-3 py-2"
                  />
                </div>
              </div>
              <div className="flex gap-3 mt-6">
                <button type="button" onClick={() => setShowModal(false)} className="flex-1 px-4 py-2 border border-gray-300 rounded-lg hover:bg-gray-50">
                  Cancel
                </button>
                <button type="submit" className="flex-1 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700">
                  Submit
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </Layout>
  );
};