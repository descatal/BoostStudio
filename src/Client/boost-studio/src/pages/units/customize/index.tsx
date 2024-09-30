import React, { useCallback, useEffect, useState } from "react"
import { UnitDto } from "@/api/exvs"
import { fetchUnitById } from "@/api/wrapper/units-api"
import { MainNav } from "@/pages/units/customize/components/main-nav"
import UnitSwitcher from "@/pages/units/customize/components/unit-switcher"
import { useCustomizeInformationUnitStore } from "@/pages/units/customize/information/libs/store"
import { useUnitsStore } from "@/pages/units/libs/store"
import {
  generatePath,
  matchPath,
  Outlet,
  useLocation,
  useNavigate,
  useParams,
} from "react-router-dom"

const routes = ["/units/:unitId/customize/info/:tab"]

const CustomizeUnitPage = () => {
  const { selectedUnits, setSelectedUnits } = useUnitsStore((state) => state)
  const selectedTab = useCustomizeInformationUnitStore(
    (state) => state.selectedTab
  )

  const params = useParams()
  const unitId = Number(params.unitId)

  const getUnitData = useCallback(async () => {
    if (!unitId) return
    const response = await fetchUnitById(unitId)
    setSelectedUnits([response])
  }, [unitId])

  useEffect(() => {
    getUnitData().catch((e) => console.log(e))
  }, [unitId])

  const navigate = useNavigate()
  const { pathname } = useLocation()
  const pathPattern = routes.find((pattern) => matchPath(pattern, pathname))

  useEffect(() => {
    if (!pathPattern || !selectedUnits) return
    const newPath = generatePath(pathPattern, {
      ...params,
      unitId: selectedUnits[0]?.unitId,
    })
    navigate(
      {
        pathname: `${newPath}`,
      },
      {
        replace: true,
      }
    )
  }, [selectedUnits])

  return (
    <>
      {selectedUnits && selectedUnits.length > 0 && (
        <div className="flex-col md:flex">
          <div className="sticky top-0 z-10 border-b bg-background">
            <div className="flex h-16 items-center px-4">
              <UnitSwitcher
                selectedUnits={selectedUnits}
                setSelectedUnits={setSelectedUnits}
                className={"w-[350px]"}
              />
              <MainNav className="mx-6" />
            </div>
          </div>
          <Outlet />
        </div>
      )}
    </>
  )
}

export default CustomizeUnitPage
