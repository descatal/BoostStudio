import { createFileRoute } from "@tanstack/react-router";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import React from "react";
import PackFhmForm from "@/features/fhm/components/pack-fhm-form";
import UnpackFhmForm from "@/features/fhm/components/unpack-fhm-form";

export const Route = createFileRoute("/tools/fhm")({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <>
      <div className="flex items-center justify-between space-y-2">
        <h2 className="text-3xl font-bold tracking-tight">Fhm</h2>
      </div>
      <label className="text-sm text-muted-foreground">
        Pack / Unpack .fhm file
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
                  Pack asset files to .fhm container format.
                </CardDescription>
              </CardHeader>
              <CardContent>
                <PackFhmForm />
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
                  Unpack asset files from .fhm container format.
                </CardDescription>
              </CardHeader>
              <CardContent>
                <UnpackFhmForm />
              </CardContent>
            </Card>
          </div>
        </TabsContent>
      </Tabs>
    </>
  );
}
