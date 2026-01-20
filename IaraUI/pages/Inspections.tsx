import React, { useState, useEffect } from 'react';
import { Layout } from '../components/Layout';
import { ShieldCheck, Calendar, Leaf, Plus, X, AlertCircle } from 'lucide-react';
import { Inspection, Vessel, Personnel } from '../types';
import { inspectionsApi, vesselsApi, personnelApi, CreateInspectionRequest } from '../services/api';

export const Inspections: React.FC = () => {
  const [inspections, setInspections] = useState<Inspection[]>([]);
  const [vessels, setVessels] = useState<Vessel[]>([]);
  const [inspectors, setInspectors] = useState<Personnel[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showModal, setShowModal] = useState(false);
  const [formData, setFormData] = useState<CreateInspectionRequest>({
    vesselId: 0,
    inspectorId: 0,
    inspectionDate: new Date().toISOString().split('T')[0],
    isLegal: true,
    notes: ''
  });

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      setLoading(true);
      const [inspectionsData, vesselsData, personnelData] = await Promise.all([
        inspectionsApi.getAll(),
        vesselsApi.getAll(),
        personnelApi.getByRole('Inspector')
      ]);
      setInspections(inspectionsData);
      setVessels(vesselsData);
      setInspectors(personnelData);
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
      await inspectionsApi.create(formData);
      setShowModal(false);
      loadData();
      setFormData({ vesselId: 0, inspectorId: 0, inspectionDate: new Date().toISOString().split('T')[0], isLegal: true, notes: '' });
    } catch (err) {
      setError('Failed to create inspection');
    }
  };

  const getVesselName = (vesselId: number) => {
    const vessel = vessels.find(v => v.vesselId === vesselId);
    return vessel?.vesselName || `Vessel #${vesselId}`;
  };

  return (
    <Layout>
      <div className="mb-6 bg-white p-6 rounded-xl shadow-sm border border-gray-100">
        <div className="flex justify-between items-center">
          <div>
            <h1 className="text-3xl font-bold text-gray-900">Inspection & Violations</h1>
            <p className="text-gray-500 mt-1">Inspectors can log checks, register violations, and issue official acts.</p>
          </div>
          <button 
            onClick={() => setShowModal(true)}
            className="bg-blue-600 hover:bg-blue-700 text-white px-6 py-2.5 rounded-lg font-medium transition-colors shadow-sm flex items-center gap-2"
          >
            <Plus className="w-4 h-4" />
            New Inspection
          </button>
        </div>
      </div>

      {error && (
        <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg mb-6 flex items-center gap-2">
          <AlertCircle className="w-5 h-5" />
          {error}
        </div>
      )}

      <div className="flex flex-col lg:flex-row gap-6">
        {/* Inspections List */}
        <div className="flex-1">
          <div className="bg-white border border-gray-200 rounded-xl overflow-hidden shadow-sm">
            <div className="overflow-x-auto">
              <table className="min-w-full divide-y divide-gray-200">
                <thead className="bg-blue-50">
                  <tr>
                    <th className="px-6 py-4 text-left text-xs font-semibold text-blue-600 uppercase tracking-wider">ID</th>
                    <th className="px-6 py-4 text-left text-xs font-semibold text-blue-600 uppercase tracking-wider">Vessel</th>
                    <th className="px-6 py-4 text-left text-xs font-semibold text-blue-600 uppercase tracking-wider">Date</th>
                    <th className="px-6 py-4 text-center text-xs font-semibold text-blue-600 uppercase tracking-wider">Status</th>
                    <th className="px-6 py-4 text-left text-xs font-semibold text-blue-600 uppercase tracking-wider">Notes</th>
                  </tr>
                </thead>
                <tbody className="bg-white divide-y divide-gray-200">
                  {loading ? (
                    <tr><td colSpan={5} className="px-6 py-8 text-center text-gray-500">Loading...</td></tr>
                  ) : inspections.length === 0 ? (
                    <tr><td colSpan={5} className="px-6 py-8 text-center text-gray-500">No inspections found</td></tr>
                  ) : inspections.map((inspection) => (
                    <tr key={inspection.inspectionId} className="hover:bg-gray-50 transition-colors">
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{inspection.inspectionId}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900 font-medium">{getVesselName(inspection.vesselId)}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{new Date(inspection.inspectionDate).toLocaleDateString()}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-center">
                        <span className={`inline-flex items-center px-3 py-1 rounded-full text-xs font-medium ${
                          inspection.isLegal
                            ? 'bg-green-100 text-green-800' 
                            : 'bg-red-100 text-red-800'
                        }`}>
                          {inspection.isLegal ? 'Legal' : 'Violation'}
                        </span>
                      </td>
                      <td className="px-6 py-4 text-sm text-gray-500 max-w-xs truncate">{inspection.notes || '-'}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        </div>

        {/* Info Column */}
        <div className="w-full lg:w-80 space-y-6">
           <div className="bg-white p-6 rounded-xl shadow-sm border border-gray-100 flex flex-col items-center text-center">
              <div className="w-12 h-12 bg-blue-100 rounded-full flex items-center justify-center mb-4">
                 <ShieldCheck className="w-6 h-6 text-blue-600" />
              </div>
              <h3 className="font-bold text-gray-900 mb-2">Maritime Safety</h3>
              <p className="text-xs text-gray-500 mb-4">Ensure all safety equipment is up-to-date and accessible. A safe vessel is a compliant vessel!</p>
           </div>

           <div className="bg-white p-6 rounded-xl shadow-sm border border-gray-100 flex flex-col items-center text-center">
              <div className="w-12 h-12 bg-green-100 rounded-full flex items-center justify-center mb-4">
                 <Leaf className="w-6 h-6 text-green-600" />
              </div>
              <h3 className="font-bold text-gray-900 mb-2">Protect Our Waters</h3>
              <p className="text-xs text-gray-500 mb-4">Help maintain sustainable fishing by reporting accurately. Our ecosystem thanks you.</p>
           </div>
        </div>
      </div>

      {showModal && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
          <div className="bg-white rounded-xl p-6 w-full max-w-md">
            <div className="flex justify-between items-center mb-4">
              <h2 className="text-xl font-bold">New Inspection</h2>
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
                  <label className="block text-sm font-medium text-gray-700 mb-1">Inspector</label>
                  <select
                    value={formData.inspectorId}
                    onChange={(e) => setFormData({ ...formData, inspectorId: parseInt(e.target.value) })}
                    className="w-full border border-gray-300 rounded-lg px-3 py-2"
                    required
                  >
                    <option value={0}>Select an inspector</option>
                    {inspectors.map(inspector => (
                      <option key={inspector.personId} value={inspector.personId}>{inspector.name}</option>
                    ))}
                  </select>
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Inspection Date</label>
                  <input
                    type="date"
                    value={formData.inspectionDate}
                    onChange={(e) => setFormData({ ...formData, inspectionDate: e.target.value })}
                    className="w-full border border-gray-300 rounded-lg px-3 py-2"
                    required
                  />
                </div>
                <div className="flex items-center gap-2">
                  <input
                    type="checkbox"
                    id="isLegal"
                    checked={formData.isLegal}
                    onChange={(e) => setFormData({ ...formData, isLegal: e.target.checked })}
                    className="rounded border-gray-300"
                  />
                  <label htmlFor="isLegal" className="text-sm text-gray-700">Is Legal (compliant)</label>
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Notes</label>
                  <textarea
                    value={formData.notes || ''}
                    onChange={(e) => setFormData({ ...formData, notes: e.target.value })}
                    rows={3}
                    className="w-full border border-gray-300 rounded-lg px-3 py-2"
                    placeholder="Inspection notes..."
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