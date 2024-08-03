import React from 'react';
import {Card, CardContent, CardHeader, CardTitle} from "@/components/ui/card";
import {Link} from "react-router-dom";
import {UnitDto} from "@/api/exvs";

const UnitCard = ({unit}: { unit: UnitDto | undefined }) => {
  return (
    <div>
      {
        unit ?
          <div>
            <Link to={`/units/${unit.unitId}`}>
              <Card>
                <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                  <CardTitle className="text-sm font-medium">
                    {unit.unitId}
                  </CardTitle>
                </CardHeader>
                <CardContent>
                  <div className="text-2xl font-bold">{unit.name}</div>
                  <p className="text-xs text-muted-foreground">{unit.nameChinese}</p>
                  <p className="text-xs text-muted-foreground">{unit.nameJapanese}</p>
                </CardContent>
              </Card>
            </Link>
          </div> :
          <></>
      }
    </div>
  );
};

export default UnitCard;