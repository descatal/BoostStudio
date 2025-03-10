import React, { useEffect } from "react";
import ExportDialog from "@/pages/units/customize/information/components/export-dialog";
import { useExportDialogStore } from "@/pages/units/customize/information/components/export-dialog/libs/store";
import {
  InformationTabModes,
  useCustomizeInformationUnitStore,
} from "@/pages/units/customize/information/libs/store";
import { useUnitsStore } from "@/pages/units/libs/store";
import { useLocation, useNavigate, useParams } from "@tanstack/react-router";

import { Button } from "@/components/ui/button";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";

import Ammo from "./components/tabs/ammo";
import Hitboxes from "./components/tabs/hitboxes";
import Projectiles from "./components/tabs/projectiles";
import Stats from "./components/tabs/stats";

const routes = ["/units/:unitId/customize/info/:infoTab"];

export default function UnitInformation() {
  const setOpenExportDialog = useExportDialogStore(
    (state) => state.setOpenExportDialog,
  );
  const { selectedInformationTab, setSelectedInformationTab } =
    useCustomizeInformationUnitStore((state) => state);
  const { selectedUnits } = useUnitsStore((state) => state);

  const { pathname } = useLocation();
  const params = useParams();
  const navigate = useNavigate();

  const pathPattern = routes.find((pattern) => matchPath(pattern, pathname));

  useEffect(() => {
    setSelectedInformationTab(
      (params.infoTab as InformationTabModes) ?? "stats",
    );
  }, [params.infoTab]);

  return (
    <>
      <ExportDialog />
      {selectedUnits && selectedUnits.length > 0 && (
        <div className="flex-col md:flex">
          <div className="flex-1 space-y-4 p-8 pt-6">
            <div className="flex items-center justify-between space-y-2">
              <h2 className="text-3xl font-bold tracking-tight">
                {selectedUnits[0].nameEnglish}
              </h2>
              <div className="flex items-center space-x-2">
                <Button
                  onClick={() => {
                    setOpenExportDialog(true);
                  }}
                >
                  Import
                </Button>
                <Button
                  onClick={() => {
                    setOpenExportDialog(true);
                  }}
                >
                  Export
                </Button>
              </div>
            </div>
            <label className="text-sm text-muted-foreground">
              UnitId: {selectedUnits[0].unitId}
            </label>
            <Tabs
              defaultValue="stats"
              className="space-y-4"
              value={selectedInformationTab}
              onValueChange={(value: string) => {
                if (!pathPattern) return;
                const newPath = generatePath(pathPattern, {
                  ...params,
                  infoTab: value,
                });
                navigate(
                  {
                    pathname: `${newPath}`,
                  },
                  {
                    replace: true,
                  },
                );
              }}
            >
              <TabsList>
                <TabsTrigger value="stats">Stats</TabsTrigger>
                <TabsTrigger value="ammo">Ammo</TabsTrigger>
                <TabsTrigger value="projectiles">Projectiles</TabsTrigger>
                <TabsTrigger value="hitboxes">Hitboxes</TabsTrigger>
              </TabsList>
              <TabsContent value="stats" className="space-y-4">
                <Stats unitId={selectedUnits[0].unitId!} />
              </TabsContent>
              <TabsContent value="ammo" className="space-y-4">
                <Ammo unitId={selectedUnits[0].unitId!} />
              </TabsContent>
              <TabsContent value="projectiles" className="space-y-4">
                <Projectiles unitId={selectedUnits[0].unitId!} />
              </TabsContent>
              <TabsContent value="hitboxes" className="space-y-4">
                <Hitboxes unitId={selectedUnits[0].unitId!} />
              </TabsContent>
            </Tabs>
          </div>
        </div>
      )}
    </>
  );
}
