import React from 'react';
import { Layout } from '../components/Layout';
import { Plus, Search, Calendar, Filter, Edit2, Trash2, ChevronLeft, ChevronRight } from 'lucide-react';

export const Quotas: React.FC = () => {
  const quotas = [
    { id: 'BG-PERMIT-001', species: 'European Sprat', year: 2024, min: '5,000', avg: '7,500', max: '10,000', fuel: 200 },
    { id: 'BG-PERMIT-002', species: 'Turbot', year: 2024, min: '1,000', avg: '1,500', max: '2,000', fuel: 150 },
    { id: 'BG-PERMIT-003', species: 'Red Mullet', year: 2023, min: '3,000', avg: '4,000', max: '5,000', fuel: 180 },
    { id: 'BG-PERMIT-004', species: 'Rapana Venosa', year: 2024, min: '15,000', avg: '20,000', max: '25,000', fuel: 300 },
    { id: 'BG-PERMIT-005', species: 'European Sprat', year: 2023, min: '4,500', avg: '7,000', max: '9,500', fuel: 190 },
  ];

  return (
    <Layout>
      <div className="flex flex-col md:flex-row justify-between items-start md:items-center mb-8 gap-4">
        <div>
          <h1 className="text-3xl font-bold text-gray-900">Catch Quotas Management</h1>
          <p className="text-gray-500 mt-1">View, add, and edit catch quotas for all permits.</p>
        </div>
        <button className="bg-green-400 hover:bg-green-500 text-gray-900 font-bold px-6 py-2.5 rounded-full flex items-center shadow-sm transition-colors">
          <Plus className="w-5 h-5 mr-2" />
          Add New Quota
        </button>
      </div>

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
              {quotas.map((quota, idx) => (
                <tr key={idx} className="hover:bg-gray-50 transition-colors">
                  <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{quota.id}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-600">{quota.species}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-600">{quota.year}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-600 text-center">{quota.min}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-600 text-center">{quota.avg}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-600 text-center">{quota.max}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-600 text-center">{quota.fuel}</td>
                  <td className="px-6 py-4 whitespace-nowrap text-center text-sm font-medium">
                    <div className="flex justify-center space-x-3">
                      <button className="text-gray-400 hover:text-blue-600 transition-colors">
                        <Edit2 className="w-5 h-5" />
                      </button>
                      <button className="text-gray-400 hover:text-red-600 transition-colors">
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
         <p className="text-sm text-gray-500">Showing <span className="font-bold text-gray-900">1</span> to <span className="font-bold text-gray-900">5</span> of <span className="font-bold text-gray-900">20</span> results</p>
         <div className="flex space-x-2">
            <button className="p-2 rounded-full border border-gray-200 bg-white hover:bg-gray-50 text-gray-500"><ChevronLeft className="w-5 h-5" /></button>
            <button className="p-2 rounded-full border border-gray-200 bg-white hover:bg-gray-50 text-gray-500"><ChevronRight className="w-5 h-5" /></button>
         </div>
      </div>
    </Layout>
  );
};