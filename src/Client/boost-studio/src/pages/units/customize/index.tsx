import React, { useCallback, useEffect } from "react"
import { fetchUnitById } from "@/api/wrapper/units-api"
import { MainNav } from "@/pages/units/customize/components/main-nav"
import UnitSwitcher from "@/pages/units/customize/components/unit-switcher"
import { useUnitsStore } from "@/pages/units/libs/store"
import { Outlet, useParams } from "react-router-dom"

const CustomizeUnitPage = () => {
  const { selectedUnit, setSelectedUnit } = useUnitsStore((state) => state)

  const params = useParams()
  const unitId = Number(params.unitId)

  const getUnitData = useCallback(async () => {
    if (!unitId) return
    const response = await fetchUnitById(unitId)
    setSelectedUnit(response)
  }, [unitId])

  useEffect(() => {
    getUnitData().catch((e) => console.log(e))
  }, [unitId])

  return (
    <>
      {selectedUnit && (
        <div className="flex-col md:flex">
          <div className="sticky top-0 z-10 border-b bg-background">
            <div className="flex h-16 items-center px-4">
              <UnitSwitcher unitId={selectedUnit.unitId!} />
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
