import { createFileRoute, Outlet, useMatchRoute } from "@tanstack/react-router";
import React, { useEffect } from "react";
import {
  UnitCustomizableInfoSections,
  UnitCustomizableSections,
} from "@/lib/constants";
import ExportDialog from "@/pages/units/customize/information/components/export-dialog";
import { Tabs, TabsContent, TabsList } from "@/components/ui/tabs";
import { ScrollArea, ScrollBar } from "@/components/ui/scroll-area";
import { TabsLinkTrigger } from "@/components/tabs-link-trigger";
import { useAppContext } from "@/providers/app-store-provider";

export const Route = createFileRoute("/units/info")({
  component: RouteComponent,
});

function RouteComponent() {
  const matchRoute = useMatchRoute();
  const matchedSection =
    Object.values(UnitCustomizableInfoSections).find((path) => {
      return matchRoute({ to: `/units/info/${path}` });
    }) ?? UnitCustomizableInfoSections.Stats;

  return (
    <>
      <ExportDialog />
      <div className="flex-col md:flex">
        <div className="flex-1 space-y-4 p-8 pt-6">
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
    </>
  );
}
