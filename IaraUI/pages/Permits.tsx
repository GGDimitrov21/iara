import React from 'react';
import { Layout } from '../components/Layout';
import { Search, Fish, MoreHorizontal } from 'lucide-react';
import { FishingPermit } from '../types';

export const Permits: React.FC = () => {
  const permits: FishingPermit[] = [
    { id: '1', permitNumber: '78901', vesselName: 'Fishing Vessel Elena', issueDate: '2023-01-01', expiryDate: '2023-12-31', status: 'Active' },
    { id: '2', permitNumber: '23456', vesselName: 'Fishing Vessel Sofia', issueDate: '2023-03-15', expiryDate: '2024-03-14', status: 'Active' },
    { id: '3', permitNumber: '34567', vesselName: 'Fishing Vessel Maria', issueDate: '2022-11-01', expiryDate: '2023-10-31', status: 'Expired' },
    { id: '4', permitNumber: '45678', vesselName: 'Fishing Vessel Ivan', issueDate: '2023-05-20', expiryDate: '2024-05-19', status: 'Active' },
    { id: '5', permitNumber: '56789', vesselName: 'Fishing Vessel Georgi', issueDate: '2023-02-10', expiryDate: '2024-02-09', status: 'Active' },
  ];

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
        <button className="bg-blue-600 hover:bg-blue-700 text-white px-6 py-2.5 rounded-lg font-medium transition-colors shadow-sm">
          Issue New Permit
        </button>
      </div>

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
              {permits.map((permit) => (
                <tr key={permit.id} className="hover:bg-gray-50 transition-colors">
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">{permit.permitNumber}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900 font-medium">{permit.vesselName}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{permit.issueDate}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">{permit.expiryDate}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-center">
                    <span className={`inline-flex items-center px-3 py-1 rounded-full text-xs font-medium ${
                      permit.status === 'Active' 
                        ? 'bg-blue-100 text-blue-800' 
                        : 'bg-red-100 text-red-800'
                    }`}>
                      {permit.status}
                    </span>
                  </td>
                  <td className="px-6 py-4 whitespace-nowrap text-right text-sm font-medium">
                    <button className="text-blue-600 hover:text-blue-900 font-semibold">View</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </Layout>
  );
};