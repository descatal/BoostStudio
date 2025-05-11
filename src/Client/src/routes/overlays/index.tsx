import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { createFileRoute } from "@tanstack/react-router";
import React, { useEffect, useState } from "react";
import { EnhancedButton } from "@/components/ui/enhanced-button";
import { Check, Layers, X } from "lucide-react";
import { listen } from "@tauri-apps/api/event";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import { invoke } from "@tauri-apps/api/core";

export const Route = createFileRoute("/overlays/")({
  component: RouteComponent,
});

type OverlayListenerStarted = {
  listener_interval: number;
  target_window_titles: string[];
};

type OverlayListenerProgress = {
  target_window_found: boolean;
};

type OverlayListenerStopped = {};

function RouteComponent() {
  const query = useQuery({
    queryKey: ["overlay_listener"],
    queryFn: async (): Promise<boolean> => {
      return await invoke("get_listener");
    },
  });

  const isListenerStarted = query.data;
  const queryClient = useQueryClient();

  const [appFound, setAppFound] = useState(false);

  useEffect(() => {
    const start = listen<OverlayListenerStarted>("overlay-started", (event) => {
      console.log(event);
      queryClient.setQueryData(["overlay_listener"], true);
    });

    const progress = listen<OverlayListenerProgress>(
      "overlay-progress",
      (event) => {
        console.log(event);
        setAppFound(event.payload.target_window_found);
      },
    );

    const stopped = listen<OverlayListenerStopped>(
      "overlay-stopped",
      (event) => {
        console.log(event);
        queryClient.setQueryData(["overlay_listener"], false);
      },
    );

    return () => {
      start.then();
      progress.then();
      stopped.then();
    };
  }, []);

  return (
    <div className="container max-w-4xl py-10 p-8 pt-6">
      <Card>
        <CardHeader>
          <div className="flex items-center justify-between">
            <div>
              <CardTitle>Overlay Session</CardTitle>
              <CardDescription>Control the overlay listener</CardDescription>
            </div>
            {isListenerStarted ? (
              appFound ? (
                <div className="inline-flex items-center rounded-full border px-2.5 py-0.5 text-xs font-semibold bg-green-100 text-green-800 border-green-200">
                  <Check className="h-3 w-3 mr-1" />
                  Window detected - Overlay active
                </div>
              ) : (
                <div className="inline-flex items-center rounded-full border px-2.5 py-0.5 text-xs font-semibold bg-red-100 text-red-800 border-red-200">
                  <X className="h-3 w-3 mr-1" />
                  Window not detected
                </div>
              )
            ) : (
              <div className="inline-flex items-center rounded-full border px-2.5 py-0.5 text-xs font-semibold bg-slate-100 text-slate-800 border-slate-200 dark:bg-slate-800 dark:text-slate-300 dark:border-slate-700">
                Listener stopped
              </div>
            )}
          </div>
        </CardHeader>
        <CardContent>
          <EnhancedButton
            icon={Layers}
            iconPlacement={"left"}
            variant={isListenerStarted ? "destructive" : "default"}
            onClick={async () => {
              if (isListenerStarted) {
                await invoke("stop_listening");
              } else {
                await invoke("start_listening", {
                  interval: 500,
                  keywords: ["NPJB00512", "BLJS10250"],
                });
              }
            }}
            className="w-full"
          >
            {isListenerStarted ? "Stop Listener" : "Start Listener"}
          </EnhancedButton>
        </CardContent>
      </Card>
    </div>
  );
}
