import {
  createFileRoute,
  Link,
  Outlet,
  useMatchRoute,
} from "@tanstack/react-router";
import { UnitCustomizableInfoSections } from "@/lib/constants";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { ScrollArea, ScrollBar } from "@/components/ui/scroll-area";

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
                  <TabsTrigger key={value} value={value}>
                    <Link to={`/units/info/${value}`}>{label}</Link>
                  </TabsTrigger>
                ),
              )}
            </TabsList>
          </div>
          <ScrollBar orientation="horizontal" />
        </ScrollArea>
        {Object.entries(UnitCustomizableInfoSections).map(([_, value]) => (
          <TabsContent key={value} value={value}>
            <Outlet />
          </TabsContent>
        ))}
      </Tabs>
    </>
  );
}
