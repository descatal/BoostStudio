import React, { useCallback, useEffect } from "react";
import { UnitSummaryVm } from "@/api/exvs";
import { fetchUnitById } from "@/api/wrapper/units-api";
import SeriesUnitsSelector from "@/features/series/components/series-units-selector";
import { CustomizeUnitNav } from "@/pages/units/customize/components/customize-unit-nav";
import { CustomizeSections, useUnitsStore } from "@/pages/units/libs/store";
import {
  Outlet,
  useLocation,
  useNavigate,
  useParams,
} from "@tanstack/react-router";
import TopBar from "@/components/custom/top-bar";

const routes = ["/units/:unitId/customize/*"];

const CustomizeUnitPage = () => {
  const {
    selectedUnits,
    setSelectedUnits,
    customizeSection,
    setCustomizeSection,
  } = useUnitsStore((state) => state);

  const params = useParams();
  const location = useLocation();
  const navigate = useNavigate();

  const unitId = Number(params.unitId);
  const pathname = location.pathname;
  const pathPattern = routes.find((pattern) => matchPath(pattern, pathname));

  React.useEffect(() => {
    // Possible sections
    let section: CustomizeSections = "info";

    // Determine which section we're in
    for (const sec of CustomizeSections) {
      if (pathname.includes(`/units/${unitId}/customize/${sec}`)) {
        section = sec;
        break;
      }
    }

    setCustomizeSection(section);
  }, [pathname, unitId]);

  const getUnitData = useCallback(async () => {
    if (!unitId) return;
    const response = await fetchUnitById(unitId);
    setSelectedUnits([response]);
  }, [unitId]);

  useEffect(() => {
    setSelectedUnits([]);
    getUnitData().catch((e) => console.log(e));
  }, []);

  return (
    <>
      {unitId && (
        <div className="flex-col md:flex">
          <TopBar>
            <SeriesUnitsSelector
              selectedUnits={selectedUnits}
              setSelectedUnits={(
                selectedUnits: UnitSummaryVm[] | undefined,
              ) => {
                if (
                  !pathPattern ||
                  !selectedUnits ||
                  selectedUnits.length <= 0 ||
                  unitId == selectedUnits[0]?.unitId
                ) {
                  return;
                }
                const newPath = generatePath(pathPattern, {
                  ...params,
                  unitId: selectedUnits[0]?.unitId,
                });
                navigate(
                  {
                    pathname: `${newPath}/${customizeSection}`,
                  },
                  {
                    replace: false,
                  },
                );
                setSelectedUnits(selectedUnits);
              }}
              className={"w-[350px]"}
            />
            <div className={"mr-4 flex w-full flex-row justify-between"}>
              <CustomizeUnitNav className="mx-6" />
            </div>
          </TopBar>
          <Outlet />
        </div>
      )}
    </>
  );
};

export default CustomizeUnitPage;
