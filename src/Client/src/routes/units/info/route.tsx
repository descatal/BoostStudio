import { createFileRoute, Outlet, useMatchRoute } from "@tanstack/react-router";
import React from "react";
import { UnitCustomizableInfoSections } from "@/lib/constants";
import { Tabs, TabsContent, TabsList } from "@/components/ui/tabs";
import { ScrollArea, ScrollBar } from "@/components/ui/scroll-area";
import { TabsLinkTrigger } from "@/components/tabs-link-trigger";

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
        {Object.entries(UnitCustomizableInfoSections).map(([label, value]) => (
          <TabsContent key={value} value={value}>
            <Outlet />
          </TabsContent>
        ))}
      </Tabs>
    </>
  );
}
