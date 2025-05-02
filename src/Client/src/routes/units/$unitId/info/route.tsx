import { createFileRoute, Outlet, useMatchRoute } from "@tanstack/react-router";
import { Button } from "@/components/ui/button";
import { Tabs, TabsContent, TabsList } from "@/components/ui/tabs";
import React from "react";
import { ScrollArea, ScrollBar } from "@/components/ui/scroll-area";
import { TabsLinkTrigger } from "@/components/tabs-link-trigger";
import { UnitCustomizableInfoSections } from "@/lib/constants";
import { useQuery } from "@tanstack/react-query";
import { getApiUnitsByUnitIdOptions } from "@/api/exvs/@tanstack/react-query.gen";

export const Route = createFileRoute("/units/$unitId/info")({
  component: RouteComponent,
});

function RouteComponent() {
  const { unitId }: { unitId: number } = Route.useParams();

  const query = useQuery({
    ...getApiUnitsByUnitIdOptions({
      path: {
        unitId: unitId,
      },
    }),
  });

  const data = query.data;

  const matchRoute = useMatchRoute();
  const matchedSection =
    Object.values(UnitCustomizableInfoSections).find((path) => {
      return matchRoute({ to: `/units/$unitId/info/${path}` });
    }) ?? UnitCustomizableInfoSections.Stats;

  return (
    <>
      {/*<ExportDialog />*/}
      {data && (
        <div className="flex-col md:flex">
          <div className="flex-1 space-y-4 p-8 pt-6">
            <div className="flex items-center justify-between space-y-2">
              <h2 className="text-3xl font-bold tracking-tight">
                {data.nameEnglish}
              </h2>
              <div className="flex items-center space-x-2">
                <Button
                  onClick={() => {
                    //setOpenExportDialog(true);
                  }}
                >
                  Import
                </Button>
                <Button
                  onClick={() => {
                    //setOpenExportDialog(true);
                  }}
                >
                  Export
                </Button>
              </div>
            </div>
            <label className="text-sm text-muted-foreground">
              UnitId: {data.unitId}
            </label>
            <Tabs className="space-y-4" value={matchedSection}>
              <ScrollArea>
                <div className="relative h-10 w-full">
                  <TabsList className="absolute flex h-10">
                    {Object.entries(UnitCustomizableInfoSections).map(
                      ([label, value]) => (
                        <TabsLinkTrigger key={value} href={value}>
                          {label}
                        </TabsLinkTrigger>
                      ),
                    )}
                  </TabsList>
                </div>
                <ScrollBar orientation="horizontal" />
              </ScrollArea>
              {Object.entries(UnitCustomizableInfoSections).map(
                ([label, value]) => (
                  <TabsContent key={value} value={value}>
                    <Outlet />
                  </TabsContent>
                ),
              )}
            </Tabs>
          </div>
        </div>
      )}
    </>
  );
}
