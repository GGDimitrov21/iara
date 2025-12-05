import React from 'react';
import { Layout } from '../components/Layout';
import { Calendar } from 'lucide-react';

export const Operations: React.FC = () => {
  return (
    <Layout>
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-gray-900 text-blue-900">Fishing Operations Log</h1>
        <p className="text-gray-500">Record your daily fishing activities with precision and detail.</p>
      </div>

      <div className="bg-cyan-50 min-h-[calc(100vh-200px)] rounded-3xl p-8 relative overflow-hidden">
        {/* Subtle map background effect */}
        <div className="absolute inset-0 opacity-10 pointer-events-none" style={{ backgroundImage: 'radial-gradient(#0891b2 1px, transparent 1px)', backgroundSize: '30px 30px' }}></div>

        <div className="max-w-4xl mx-auto relative z-10">
          <div className="mb-8">
            <h2 className="text-xl font-bold text-blue-900 mb-4 border-b border-blue-200 pb-2">Start of Fishing</h2>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div>
                <label className="block text-sm font-medium text-blue-800 mb-1">Vessel ID</label>
                <input type="text" placeholder="Enter vessel ID" className="w-full px-4 py-3 rounded-lg border-none shadow-sm focus:ring-2 focus:ring-blue-400 outline-none" />
              </div>
               <div>
                <label className="block text-sm font-medium text-blue-800 mb-1">Captain ID</label>
                <input type="text" placeholder="Enter captain ID" className="w-full px-4 py-3 rounded-lg border-none shadow-sm focus:ring-2 focus:ring-blue-400 outline-none" />
              </div>
              <div className="relative">
                 <label className="block text-sm font-medium text-blue-800 mb-1">Start Time</label>
                 <div className="relative">
                    <input type="text" placeholder="mm/dd/yyyy, --:-- --" className="w-full px-4 py-3 rounded-lg border-none shadow-sm focus:ring-2 focus:ring-blue-400 outline-none" />
                    <Calendar className="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-400 w-5 h-5" />
                 </div>
              </div>
               <div>
                <label className="block text-sm font-medium text-blue-800 mb-1">Duration (hours)</label>
                <input type="number" placeholder="Enter duration in hours" className="w-full px-4 py-3 rounded-lg border-none shadow-sm focus:ring-2 focus:ring-blue-400 outline-none" />
              </div>
               <div>
                <label className="block text-sm font-medium text-blue-800 mb-1">Latitude</label>
                <input type="text" placeholder="Enter latitude" className="w-full px-4 py-3 rounded-lg border-none shadow-sm focus:ring-2 focus:ring-blue-400 outline-none" />
              </div>
               <div>
                <label className="block text-sm font-medium text-blue-800 mb-1">Longitude</label>
                <input type="text" placeholder="Enter longitude" className="w-full px-4 py-3 rounded-lg border-none shadow-sm focus:ring-2 focus:ring-blue-400 outline-none" />
              </div>
            </div>
          </div>

          <div className="mb-8">
            <h2 className="text-xl font-bold text-blue-900 mb-4 border-b border-blue-200 pb-2">Catch Details</h2>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
               <div>
                <label className="block text-sm font-medium text-blue-800 mb-1">Species ID</label>
                <input type="text" placeholder="Enter species ID" className="w-full px-4 py-3 rounded-lg border-none shadow-sm focus:ring-2 focus:ring-blue-400 outline-none" />
              </div>
               <div>
                <label className="block text-sm font-medium text-blue-800 mb-1">Catch (kg)</label>
                <input type="number" placeholder="Enter catch in kilograms" className="w-full px-4 py-3 rounded-lg border-none shadow-sm focus:ring-2 focus:ring-blue-400 outline-none" />
              </div>
            </div>
          </div>

          <div className="mb-8">
            <h2 className="text-xl font-bold text-blue-900 mb-4">Unloaded Fish History</h2>
            <div className="bg-white rounded-lg shadow-sm overflow-hidden">
               <table className="min-w-full divide-y divide-gray-200">
                <thead className="bg-gray-50">
                  <tr>
                    <th className="px-6 py-3 text-left text-xs font-bold text-gray-500 uppercase tracking-wider">Date</th>
                    <th className="px-6 py-3 text-left text-xs font-bold text-gray-500 uppercase tracking-wider">Species</th>
                    <th className="px-6 py-3 text-left text-xs font-bold text-gray-500 uppercase tracking-wider">Quantity (KG)</th>
                    <th className="px-6 py-3 text-left text-xs font-bold text-gray-500 uppercase tracking-wider">Location</th>
                  </tr>
                </thead>
                <tbody className="bg-white divide-y divide-gray-200">
                  {[
                    {date: '2024-07-20', species: 'Trout', qty: 50, loc: 'River Danube'},
                    {date: '2024-07-19', species: 'Carp', qty: 75, loc: 'Lake Varna'},
                    {date: '2024-07-18', species: 'Perch', qty: 30, loc: 'Black Sea'},
                  ].map((row, i) => (
                    <tr key={i}>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-700">{row.date}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-700">{row.species}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-700">{row.qty}</td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-700">{row.loc}</td>
                    </tr>
                  ))}
                </tbody>
               </table>
            </div>
          </div>

          <div className="flex justify-end pt-4">
             <button className="bg-blue-700 hover:bg-blue-800 text-white px-8 py-3 rounded-lg font-bold shadow-md transition-colors">
               Submit Log
             </button>
          </div>
        </div>
      </div>
    </Layout>
  );
};