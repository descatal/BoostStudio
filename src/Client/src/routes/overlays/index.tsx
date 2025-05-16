import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { createFileRoute } from "@tanstack/react-router";
import { useEffect, useState } from "react";
import { EnhancedButton } from "@/components/ui/enhanced-button";
import { Check, Layers, X } from "lucide-react";
import { listen } from "@tauri-apps/api/event";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { invoke, isTauri } from "@tauri-apps/api/core";
import OverlayAdvancedSettingsForm, {
  OVERLAY_SETTINGS,
  OverlaySettings,
} from "@/features/overlays/components/forms/overlay-advanced-settings-form";
import { Tooltip } from "@/components/ui/tooltip.tsx";
import { Badge } from "@/components/ui/badge.tsx";
import { getApiConfigsByKeyOptions } from "@/api/exvs/@tanstack/react-query.gen.ts";
import { toast } from "sonner";

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

  const settingsQuery = useQuery({
    ...getApiConfigsByKeyOptions({
      path: {
        key: OVERLAY_SETTINGS,
      },
    }),
    select: (data) => {
      return JSON.parse(data) as OverlaySettings;
    },
  });

  const isListenerStarted = query.data;
  const queryClient = useQueryClient();

  const tauri = isTauri();
  const [appFound, setAppFound] = useState(false);

  useEffect(() => {
    if (!tauri) return;

    const start = listen<OverlayListenerStarted>("overlay-started", (_) => {
      queryClient.setQueryData(["overlay_listener"], true);
    });

    const progress = listen<OverlayListenerProgress>(
      "overlay-progress",
      (event) => {
        setAppFound(event.payload.target_window_found);
      },
    );

    const stopped = listen<OverlayListenerStopped>("overlay-stopped", (_) => {
      queryClient.setQueryData(["overlay_listener"], false);
    });

    return () => {
      start.then();
      progress.then();
      stopped.then();
    };
  }, [tauri]);

  const startListenerMutation = useMutation({
    mutationFn: async () => {
      if (!settingsQuery.data) {
        toast.error(
          "Failed to start listener, no settings has been retrieved!",
        );
        return;
      }

      const request = {
        interval: settingsQuery.data?.interval ?? 300,
        keywords: settingsQuery.data?.windowTitles ?? [""],
      };
      await invoke("start_listening", request);
      toast.message("Listener started!");
    },
  });

  const stopListenerMutation = useMutation({
    mutationFn: async () => {
      await invoke("stop_listening");
      toast.message("Listener stopped!");
    },
  });

  async function handleListenerOnClick() {
    if (tauri) return;

    if (isListenerStarted) {
      stopListenerMutation.mutate();
    } else {
      startListenerMutation.mutate();
    }
  }

  return (
    <div className="flex justify-center">
      <div className="w-4xl py-10 p-8 pt-6 grid gap-6">
        <Card>
          <CardHeader>
            <div className="flex items-center justify-between">
              <div>
                <CardTitle>Overlay Session</CardTitle>
                <CardDescription>Control the overlay listener</CardDescription>
              </div>
              {isListenerStarted ? (
                appFound ? (
                  <Badge variant={"default"}>
                    <Check className="h-3 w-3 mr-1" />
                    Window detected - Overlay active
                  </Badge>
                ) : (
                  <Badge variant={"destructive"}>
                    <X className="h-3 w-3 mr-1" />
                    Window not detected
                  </Badge>
                )
              ) : tauri ? (
                <Badge variant={"destructive"}>Listener stopped</Badge>
              ) : (
                <Badge variant={"destructive"}>Not supported in browser</Badge>
              )}
            </div>
          </CardHeader>
          <CardContent>
            <Tooltip></Tooltip>
            <EnhancedButton
              disabled={!tauri}
              icon={Layers}
              iconPlacement={"right"}
              variant={isListenerStarted ? "destructive" : "default"}
              onClick={handleListenerOnClick}
              className="w-full"
            >
              {isListenerStarted ? "Stop Listener" : "Start Listener"}
            </EnhancedButton>
          </CardContent>
        </Card>
        <Card>
          <CardHeader>
            <div className="flex items-center justify-between">
              <div>
                <CardTitle>Customize Overlay</CardTitle>
                <CardDescription>
                  {isListenerStarted
                    ? "Stop the listener to customize overlay listener settings"
                    : "Adjust settings on the overlay listener"}
                </CardDescription>
              </div>
              {isListenerStarted && (
                <div className="inline-flex items-center rounded-full border px-2.5 py-0.5 text-xs font-semibold bg-yellow-100 text-yellow-800 border-yellow-200">
                  <span>Locked while listener is active</span>
                </div>
              )}
            </div>
          </CardHeader>
          <CardContent>
            <OverlayAdvancedSettingsForm />
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
