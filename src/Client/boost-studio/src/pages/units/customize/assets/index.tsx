import React from "react"
import AssetTools from "@/pages/tools/assets"
import { useUnitsStore } from "@/pages/units/libs/store"

const UnitAssets = () => {
  const { selectedUnits } = useUnitsStore((state) => state)

  return (
    <div className="flex justify-center space-y-4 p-8 pt-6">
      <AssetTools units={selectedUnits} />
    </div>
  )
}

export default UnitAssets
