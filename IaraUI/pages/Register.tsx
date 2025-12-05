import React from 'react';
import { Link, useNavigate } from 'react-router-dom';

export const Register: React.FC = () => {
  const navigate = useNavigate();

  const handleRegister = (e: React.FormEvent) => {
    e.preventDefault();
    navigate('/dashboard');
  };

  return (
    <div className="min-h-screen bg-gray-50 flex flex-col items-center pt-10 px-4">
      {/* Banner */}
      <div className="w-full max-w-4xl h-64 rounded-3xl overflow-hidden shadow-lg mb-8 relative bg-[#faebd7]">
         <img 
           src="https://picsum.photos/1200/400?random=11" 
           alt="Ship illustration" 
           className="w-full h-full object-cover mix-blend-multiply opacity-80"
         />
      </div>

      <div className="w-full max-w-lg">
        <h1 className="text-3xl font-bold text-center text-gray-900 mb-8">Create your account</h1>
        
        <form onSubmit={handleRegister} className="space-y-4">
          <div>
            <input 
              type="text" 
              placeholder="Full Name" 
              className="w-full px-4 py-3 bg-gray-200 bg-opacity-50 border-none rounded-lg focus:ring-2 focus:ring-blue-500 focus:bg-white transition-colors"
            />
          </div>

          <div>
            <div className="relative">
              <select className="w-full px-4 py-3 bg-gray-200 bg-opacity-50 border-none rounded-lg focus:ring-2 focus:ring-blue-500 focus:bg-white transition-colors appearance-none text-gray-600">
                <option>Role</option>
                <option>Captain</option>
                <option>Inspector</option>
                <option>Admin</option>
              </select>
               <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-4 text-gray-500">
                <svg className="fill-current h-4 w-4" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20"><path d="M9.293 12.95l.707.707L15.657 8l-1.414-1.414L10 10.828 5.757 6.586 4.343 8z"/></svg>
              </div>
            </div>
          </div>

          <div>
            <input 
              type="email" 
              placeholder="Contact Email" 
              className="w-full px-4 py-3 bg-gray-200 bg-opacity-50 border-none rounded-lg focus:ring-2 focus:ring-blue-500 focus:bg-white transition-colors"
            />
          </div>

          <div>
            <input 
              type="password" 
              placeholder="Password" 
              className="w-full px-4 py-3 bg-gray-200 bg-opacity-50 border-none rounded-lg focus:ring-2 focus:ring-blue-500 focus:bg-white transition-colors"
            />
          </div>

          <button type="submit" className="w-full bg-blue-500 hover:bg-blue-600 text-white font-bold py-3 rounded-lg transition-colors mt-4">
            Register
          </button>
        </form>

        <div className="mt-6 text-center">
           <Link to="/login" className="text-blue-600 underline text-sm">Already have an account? Sign in</Link>
        </div>
      </div>
    </div>
  );
};