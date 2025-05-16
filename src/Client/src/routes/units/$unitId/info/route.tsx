import {
  createFileRoute,
  Link,
  Outlet,
  useMatchRoute,
} from "@tanstack/react-router";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { ScrollArea, ScrollBar } from "@/components/ui/scroll-area";
import { UnitCustomizableInfoSections } from "@/lib/constants";

export const Route = createFileRoute("/units/$unitId/info")({
  component: RouteComponent,
});

function RouteComponent() {
  const { unitId }: { unitId: string } = Route.useParams();
  const matchRoute = useMatchRoute();
  const matchedSection =
    Object.values(UnitCustomizableInfoSections).find((path) => {
      return matchRoute({ to: `/units/$unitId/info/${path}` });
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
                    <Link
                      to={`/units/$unitId/info/${value}`}
                      params={{ unitId: unitId }}
                    >
                      {label}
                    </Link>
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
