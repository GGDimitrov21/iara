import React from 'react';
import { Layout } from '../components/Layout';
import { ShieldCheck, Calendar, Leaf } from 'lucide-react';

export const Inspections: React.FC = () => {
  return (
    <Layout>
      <div className="mb-6 bg-white p-6 rounded-xl shadow-sm border border-gray-100">
        <h1 className="text-3xl font-bold text-gray-900">Inspection & Violations</h1>
        <p className="text-gray-500 mt-1">Inspectors can log checks, register violations, and issue official acts.</p>
      </div>

      <div className="flex flex-col lg:flex-row gap-6">
        {/* Form Column */}
        <div className="flex-1 space-y-6">
          <div className="bg-white p-6 rounded-xl shadow-sm border border-gray-100">
             <h2 className="text-lg font-bold text-gray-900 mb-6">Inspection Details</h2>
             
             <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-6">
                <div>
                   <label className="block text-xs font-medium text-gray-700 mb-1">Inspection Date</label>
                   <div className="relative">
                      <input type="text" placeholder="mm/dd/yyyy" className="w-full px-4 py-3 bg-gray-50 rounded-lg border border-gray-200 focus:ring-1 focus:ring-blue-500 outline-none text-sm" />
                      <Calendar className="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-400 w-4 h-4" />
                   </div>
                </div>
                <div>
                   <label className="block text-xs font-medium text-gray-700 mb-1">Inspector Name</label>
                   <input type="text" placeholder="e.g., Ivan Ivanov" className="w-full px-4 py-3 bg-gray-50 rounded-lg border border-gray-200 focus:ring-1 focus:ring-blue-500 outline-none text-sm" />
                </div>
             </div>

             <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-6">
                <div>
                   <label className="block text-xs font-medium text-gray-700 mb-1">Vessel Name</label>
                   <input type="text" placeholder="e.g., The Black Pearl" className="w-full px-4 py-3 bg-gray-50 rounded-lg border border-gray-200 focus:ring-1 focus:ring-blue-500 outline-none text-sm" />
                </div>
                <div>
                   <label className="block text-xs font-medium text-gray-700 mb-1">Permit Number</label>
                   <input type="text" placeholder="e.g., BG-12345" className="w-full px-4 py-3 bg-gray-50 rounded-lg border border-gray-200 focus:ring-1 focus:ring-blue-500 outline-none text-sm" />
                </div>
             </div>

             <div>
                <label className="block text-xs font-medium text-gray-700 mb-1">Inspection Notes</label>
                <textarea rows={4} placeholder="Add any relevant notes from the inspection..." className="w-full px-4 py-3 bg-gray-50 rounded-lg border border-gray-200 focus:ring-1 focus:ring-blue-500 outline-none text-sm resize-none"></textarea>
             </div>
          </div>

          <div className="bg-white p-6 rounded-xl shadow-sm border border-gray-100">
             <h2 className="text-lg font-bold text-gray-900 mb-6">Violation Details</h2>
             <div className="mb-4">
                <label className="block text-xs font-medium text-gray-700 mb-1">Violation Type</label>
                <select className="w-full px-4 py-3 bg-gray-50 rounded-lg border border-gray-200 focus:ring-1 focus:ring-blue-500 outline-none text-sm appearance-none">
                  <option>Select a violation type</option>
                  <option>Illegal Gear</option>
                  <option>Quota Exceeded</option>
                  <option>Restricted Zone</option>
                </select>
             </div>
              <div>
                <label className="block text-xs font-medium text-gray-700 mb-1">Violation Notes</label>
                <textarea rows={4} placeholder="Enter violation details" className="w-full px-4 py-3 bg-gray-50 rounded-lg border border-gray-200 focus:ring-1 focus:ring-blue-500 outline-none text-sm resize-none"></textarea>
             </div>
          </div>

           <div className="bg-white p-6 rounded-xl shadow-sm border border-gray-100">
             <h2 className="text-lg font-bold text-gray-900 mb-6">Official Acts</h2>
             <div className="mb-4">
                <label className="block text-xs font-medium text-gray-700 mb-1">Act Type</label>
                <select className="w-full px-4 py-3 bg-gray-50 rounded-lg border border-gray-200 focus:ring-1 focus:ring-blue-500 outline-none text-sm appearance-none">
                  <option>Select an act type</option>
                  <option>Warning</option>
                  <option>Fine</option>
                  <option>Seizure</option>
                </select>
             </div>
              <div>
                <label className="block text-xs font-medium text-gray-700 mb-1">Act Notes</label>
                <textarea rows={4} placeholder="Enter act details" className="w-full px-4 py-3 bg-gray-50 rounded-lg border border-gray-200 focus:ring-1 focus:ring-blue-500 outline-none text-sm resize-none"></textarea>
             </div>
          </div>

          <div className="flex justify-end">
             <button className="bg-blue-600 hover:bg-blue-700 text-white font-bold py-3 px-8 rounded-lg transition-colors">
               Submit Inspection Report
             </button>
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
              <div className="w-full h-32 rounded-lg overflow-hidden">
                 <img src="https://picsum.photos/400/300?random=3" alt="Safety" className="w-full h-full object-cover" />
              </div>
           </div>

           <div className="bg-white p-6 rounded-xl shadow-sm border border-gray-100 flex flex-col items-center text-center">
              <div className="w-12 h-12 bg-green-100 rounded-full flex items-center justify-center mb-4">
                 <Leaf className="w-6 h-6 text-green-600" />
              </div>
              <h3 className="font-bold text-gray-900 mb-2">Protect Our Waters</h3>
              <p className="text-xs text-gray-500 mb-4">Help maintain sustainable fishing by reporting accurately. Our ecosystem thanks you.</p>
               <div className="w-full h-32 rounded-lg overflow-hidden">
                 <img src="https://picsum.photos/400/300?random=4" alt="Nature" className="w-full h-full object-cover" />
              </div>
           </div>
        </div>
      </div>
    </Layout>
  );
};