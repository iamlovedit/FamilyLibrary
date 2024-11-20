import { Outlet, useNavigate, useLocation } from 'react-router-dom';
import { useState } from 'react';
import Header from '@/components/Header';
import Footer from '@/components/Footer';

function MainLayout() {

    return (
        <div className="h-dvh w-full flex flex-col">
            <Header />
            <div className='flex-1'>
                <Outlet />
            </div>
            <Footer />
        </div>
    )
}

export default MainLayout

