import {Button} from "@/components/ui/button"

import {Tabs, TabsContent, TabsList, TabsTrigger,} from "@/components/ui/tabs"
import Stats from "@/pages/units/customize/information/tabs/stats";
import Ammo from "./tabs/ammo"
import React, {useCallback, useEffect, useState} from "react";
import {generatePath, matchPath, useLocation, useNavigate, useParams} from "react-router-dom"
import {UnitDto} from "@/api/exvs";
import {fetchUnitById, fetchUnits} from "@/api/wrapper/units-api";
import Projectiles from "./tabs/projectiles";
import Hitboxes from "./tabs/hitboxes";

const routes = [
  "/units/:unitId/info/:tab",
];

export default function UnitInformation() {
  const {pathname} = useLocation();
  const pathPattern = routes.find((pattern) => matchPath(pattern, pathname));
  const params = useParams();
  const navigate = useNavigate();

  const [unitIdState, setUnitIdState] = useState<number>()
  const [tabValue, setTabValue] = useState<string>("stats")
  const [unit, setUnit] = useState<UnitDto>();
  
  useEffect(() => {
    if (params.unitId === undefined) return
    setUnitIdState(Number(params.unitId))
  }, [params.unitId]);

  useEffect(() => {
    setTabValue(params.tab ?? "stats")
  }, [params.tab]);

  const getUnit = useCallback( async () => {
    if (unitIdState === undefined) return
    let unit = await fetchUnitById(unitIdState)
    setUnit(unit)
  }, [unitIdState]);

  useEffect(() => {
    getUnit().catch(console.log)
  }, [unitIdState]);

  return (
    <>
      {
        unit ?
          <div className="flex-col md:flex">
            <div className="flex-1 space-y-4 p-8 pt-6">
              <div className="flex items-center justify-between space-y-2">
                <h2 className="text-3xl font-bold tracking-tight">{unit?.name}</h2>
                <div className="flex items-center space-x-2">
                  <Button>Export</Button>
                </div>
              </div>
              <Tabs
                defaultValue="stats"
                className="space-y-4"
                value={tabValue}
                onValueChange={(value: string) => {
                  if (pathPattern === undefined) return
                  const newPath = generatePath(pathPattern, {...params, tab: value});
                  navigate(`${newPath}`, {replace: true})
                }}>
                <TabsList>
                  <TabsTrigger value="stats">
                    Stats
                  </TabsTrigger>
                  <TabsTrigger value="ammo">
                    Ammo
                  </TabsTrigger>
                  <TabsTrigger value="projectiles">
                    Projectiles
                  </TabsTrigger>
                  <TabsTrigger value="hitboxes">
                    Hitboxes
                  </TabsTrigger>
                </TabsList>
                <TabsContent value="stats" className="space-y-4">
                  <Stats unitId={unit.unitId!}/>
                </TabsContent>
                <TabsContent value="ammo" className="space-y-4">
                  <Ammo unitId={unit.unitId!}/>
                </TabsContent>
                <TabsContent value="projectiles" className="space-y-4">
                  <Projectiles unitId={unit.unitId!}/>
                </TabsContent>
                <TabsContent value="hitboxes" className="space-y-4">
                  <Hitboxes unitId={unit.unitId!}/>
                </TabsContent>
              </Tabs>
            </div>
          </div> : <></>
      }
    </>
  )
}