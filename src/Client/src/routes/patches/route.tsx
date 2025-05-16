import { createFileRoute, Link, Outlet } from "@tanstack/react-router";
import { Main } from "@/components/layout/main";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { ScrollArea, ScrollBar } from "@/components/ui/scroll-area";
import { PatchFileTabs } from "@/features/patches/libs/constants";

export const Route = createFileRoute("/patches")({
  component: RouteComponent,
});

function RouteComponent() {
  const { patchId }: { patchId: PatchFileTabs } = Route.useParams();

  return (
    <>
      <Main fixed>
        <div className="mb-2 flex items-center justify-between space-y-2">
          <h1 className="text-2xl font-bold tracking-tight">Patches</h1>
        </div>
        <Tabs defaultValue="Patch6" className="space-y-4" value={patchId}>
          <ScrollArea>
            <div className="relative h-10 w-full">
              <TabsList className="absolute flex h-10">
                {Object.keys(PatchFileTabs).map((patch) => (
                  <TabsTrigger key={patch} value={patch}>
                    <Link
                      to={`/patches/$patchId`}
                      params={{
                        patchId: patch,
                      }}
                    >
                      {patch}
                    </Link>
                  </TabsTrigger>
                ))}
              </TabsList>
            </div>
            <ScrollBar orientation="horizontal" />
          </ScrollArea>
          {Object.values(PatchFileTabs).map((patch) => (
            <TabsContent key={patch} value={patch}>
              <Outlet />
            </TabsContent>
          ))}
        </Tabs>
      </Main>
    </>
  );
}
