import React from "react"
import {UnitSummaryVm} from "@/api/exvs"

import {Card, CardContent, CardHeader, CardTitle} from "@/components/ui/card"

export interface UnitCardProps extends React.HTMLProps<HTMLDivElement> {
  unit: UnitSummaryVm | undefined
  selected: boolean
}

const UnitCard = ({unit, selected, ...props}: UnitCardProps) => {
  return (
    <div {...props}>
      {unit && (
        <div>
          <Card
            className={`w-72 h-36 ${selected ? "border-green-600 dark:border-green-400" : ""}`}
          >
            <CardHeader className="flex flex-row items-center justify-between space-y-0">
              <CardTitle className="text-sm font-medium">
                {unit.nameEnglish}
                <p className="text-xs text-muted-foreground">
                  {unit.unitId}
                </p>
              </CardTitle>
            </CardHeader>
            <CardContent>
              {/*<div className="text-2xl font-bold">{unit.nameEnglish}</div>*/}

              {/*<p className="text-xs text-muted-foreground">*/}
              {/*  {unit.nameJapanese}*/}
              {/*</p>*/}
            </CardContent>
          </Card>
        </div>
      )}
    </div>
  )
}

export default UnitCard