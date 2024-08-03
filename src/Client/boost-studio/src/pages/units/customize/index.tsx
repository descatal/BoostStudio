import React from 'react';
import UnitSwitcher from "@/pages/units/customize/components/unit-switcher";
import {MainNav} from "@/pages/units/customize/components/main-nav";
import {Outlet, useParams} from "react-router-dom";

const CustomizeUnitPage = () => {
  const params = useParams();

  return (
    <div className="flex-col md:flex">
      <div className="sticky top-0 z-10 border-b bg-background">
        <div className="flex h-16 items-center px-4">
          <UnitSwitcher unitId={Number(params.unitId)}/>
          <MainNav className="mx-6"/>
          <div className="ml-auto flex items-center space-x-4">
          </div>
        </div>
      </div>
      <Outlet/>
    </div>
  );
};

export default CustomizeUnitPage;