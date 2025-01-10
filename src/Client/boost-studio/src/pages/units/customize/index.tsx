import React, { useCallback, useEffect } from "react"
import { UnitSummaryVm } from "@/api/exvs"
import { fetchUnitById } from "@/api/wrapper/units-api"
import UnitSwitcher from "@/pages/common/components/custom/unit-switcher"
import { MainNav } from "@/pages/units/customize/components/main-nav"
import { CustomizeSections, useUnitsStore } from "@/pages/units/libs/store"
import {
  generatePath,
  matchPath,
  Outlet,
  useLocation,
  useNavigate,
  useParams,
} from "react-router-dom"

const routes = ["/units/:unitId/customize/*"]

const CustomizeUnitPage = () => {
  const {
    selectedUnits,
    setSelectedUnits,
    customizeSection,
    setCustomizeSection,
  } = useUnitsStore((state) => state)

  const params = useParams()
  const location = useLocation()
  const navigate = useNavigate()

  const unitId = Number(params.unitId)
  const pathname = location.pathname
  const pathPattern = routes.find((pattern) => matchPath(pattern, pathname))

  React.useEffect(() => {
    // Possible sections
    let section: CustomizeSections = "info"

    // Determine which section we're in
    for (const sec of CustomizeSections) {
      if (pathname.includes(`/units/${unitId}/customize/${sec}`)) {
        section = sec
        break
      }
    }

    setCustomizeSection(section)
  }, [pathname, unitId])

  const getUnitData = useCallback(async () => {
    if (!unitId) return
    const response = await fetchUnitById(unitId)
    setSelectedUnits([response])
  }, [unitId])

  useEffect(() => {
    setSelectedUnits([])
    getUnitData().catch((e) => console.log(e))
  }, [])

  return (
    <>
      {unitId && (
        <div className="flex-col md:flex">
          <div className="sticky top-0 z-10 border-b bg-background">
            <div className="flex h-16 items-center px-4">
              <UnitSwitcher
                selectedUnits={selectedUnits}
                setSelectedUnits={(selectedUnits: UnitSummaryVm[] | undefined) => {
                  if (
                    !pathPattern ||
                    !selectedUnits ||
                    selectedUnits.length <= 0 ||
                    unitId == selectedUnits[0]?.unitId
                  ) {
                    return
                  }
                  const newPath = generatePath(pathPattern, {
                    ...params,
                    unitId: selectedUnits[0]?.unitId,
                  })
                  navigate(
                    {
                      pathname: `${newPath}/${customizeSection}`,
                    },
                    {
                      replace: false,
                    }
                  )
                  setSelectedUnits(selectedUnits)
                }}
                className={"w-[350px]"}
              />
              <div className={"mr-4 flex w-full flex-row justify-between"}>
                <MainNav className="mx-6" />
              </div>
            </div>
          </div>
          <Outlet />
        </div>
      )}
    </>
  )
}

export default CustomizeUnitPage
