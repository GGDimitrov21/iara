import React from 'react';
import { Layout } from '../components/Layout';
import { ArrowRight } from 'lucide-react';

const ReportCard = ({ title, description, image, action }: any) => (
  <div className="bg-white rounded-2xl p-6 border border-gray-100 shadow-sm flex flex-col items-start h-full">
    <div className="w-full h-48 bg-gray-50 rounded-xl mb-6 overflow-hidden flex items-center justify-center">
      <img src={image} alt={title} className="h-32 object-contain" />
    </div>
    <h3 className="text-xl font-bold text-gray-900 mb-2">{title}</h3>
    <p className="text-gray-500 text-sm mb-6 flex-1">{description}</p>
    <button className="flex items-center space-x-2 bg-gray-50 hover:bg-gray-100 text-gray-900 px-4 py-2 rounded-lg text-sm font-medium transition-colors">
       <span>{action}</span>
       <ArrowRight className="w-4 h-4" />
    </button>
  </div>
);

export const Reports: React.FC = () => {
  return (
    <Layout>
      <div className="flex justify-between items-center mb-8">
         <div>
            <h1 className="text-3xl font-bold text-gray-900">Reports & Analytics</h1>
            <p className="text-gray-500 mt-2">Generate reports and visualize data with a splash of fun.</p>
         </div>
         <button className="bg-blue-600 hover:bg-blue-700 text-white px-6 py-2.5 rounded-lg font-medium shadow-sm flex items-center">
            New Custom Report
         </button>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <ReportCard 
          title="Vessels with Expiring Permits"
          description="Generate a report of vessels with permits expiring within a specified timeframe."
          image="https://picsum.photos/400/300?random=5"
          action="Generate Report"
        />
        <ReportCard 
          title="Recreational Fishermen by Catch"
          description="Classify recreational fishermen based on their reported catch data."
          image="https://picsum.photos/400/300?random=6"
          action="Generate Report"
        />
        <ReportCard 
          title="Vessel Classifications by Catch"
          description="Classify fishing vessels based on their reported catch data."
          image="https://picsum.photos/400/300?random=7"
          action="Generate Report"
        />
         <ReportCard 
          title="Carbon Footprint Calculation"
          description="Calculate the carbon footprint of fishing operations based on vessel data and activity."
          image="https://picsum.photos/400/300?random=8"
          action="Calculate"
        />
      </div>
    </Layout>
  );
};