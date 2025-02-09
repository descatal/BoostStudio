import React from "react"
import ScriptTools from "@/pages/tools/scripts"
import { useUnitsStore } from "@/pages/units/libs/store"

const UnitScript = () => {
  const { selectedUnits } = useUnitsStore((state) => state)

  return (
    <div className="flex justify-center space-y-4 p-8 pt-6">
      <ScriptTools units={selectedUnits} />
    </div>
  )
}

export default UnitScript
