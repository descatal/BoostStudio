import { createFileRoute } from "@tanstack/react-router";
import {
  Tabs,
  TabsContent,
  TabsList,
  TabsTrigger,
} from "@/components/ui/tabs.tsx";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card.tsx";
import PsarcForm from "@/features/psarc/forms/psarc-form.tsx";

export const Route = createFileRoute("/tools/psarc")({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <>
      <div className="flex items-center justify-between space-y-2">
        <h2 className="text-3xl font-bold tracking-tight">Psarc</h2>
      </div>
      <label className="text-sm text-muted-foreground">
        Pack / Unpack psarc file
      </label>
      <Tabs defaultValue={"pack"}>
        <TabsList>
          <TabsTrigger value="pack">Pack</TabsTrigger>
          <TabsTrigger value="unpack">Unpack</TabsTrigger>
        </TabsList>
        <TabsContent className={"w-full"} value={"pack"}>
          <div className={"flex flex-col gap-4"}>
            <Card>
              <CardHeader>
                <CardTitle>Pack Assets</CardTitle>
                <CardDescription>
                  Pack patch files to .psarc container format.
                </CardDescription>
              </CardHeader>
              <CardContent>
                <PsarcForm mode={"pack"} version={"Patch6"} />
              </CardContent>
            </Card>
          </div>
        </TabsContent>
        <TabsContent className={"w-full"} value={"unpack"}>
          <div className={"flex flex-col gap-4"}>
            <Card>
              <CardHeader>
                <CardTitle>Unpack Assets</CardTitle>
                <CardDescription>
                  Unpack patch files from .psarc container format.
                </CardDescription>
              </CardHeader>
              <CardContent>
                <PsarcForm mode={"unpack"} version={"Patch6"} />
              </CardContent>
            </Card>
          </div>
        </TabsContent>
      </Tabs>
    </>
  );
}
