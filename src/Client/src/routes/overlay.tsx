import { createFileRoute } from "@tanstack/react-router";
import { useAppContext } from "@/providers/app-store-provider";
import { useEffect, useState } from "react";
import {
  Card,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";

export const Route = createFileRoute("/overlay")({
  component: RouteComponent,
});

type GameInfo = {
  enemyHealth: number;
  enemyEx: number;
};

function RouteComponent() {
  const setShowTopbar = useAppContext((state) => state.setShowTopbar);
  const setShowSidebar = useAppContext((state) => state.setShowSidebar);
  const setTransparent = useAppContext((state) => state.setTransparent);

  const [gameInfo, setGameInfo] = useState<GameInfo>();

  useEffect(() => {
    const transparentMode = (enable: boolean) => {
      setTransparent(!enable);
      setShowTopbar(enable);
      setShowSidebar(enable);
    };
    transparentMode(false);

    return () => {
      transparentMode(true);
    };
  }, []);

  useEffect(() => {
    const eventSource = new EventSource(
      import.meta.env.VITE_SERVER_URL + "/sse/game-info",
    );

    eventSource.addEventListener("game-info", (event) => {
      const data: GameInfo = JSON.parse(event.data);
      setGameInfo(data);
    });

    // terminating the connection on component unmount
    return () => eventSource.close();
  }, []);

  return (
    <div className={"w-full h-full"}>
      <div className={"h-full justify-items-end pt-5 pr-5"}>
        <Card>
          <CardHeader className="relative">
            <CardDescription>Health Point</CardDescription>
            <CardTitle className="@[250px]/card:text-3xl text-2xl font-semibold tabular-nums">
              {gameInfo?.enemyHealth ?? 0}
            </CardTitle>
          </CardHeader>
        </Card>
        <Card>
          <CardHeader className="relative">
            <CardDescription>Ex</CardDescription>
            <CardTitle className="@[250px]/card:text-3xl text-2xl font-semibold tabular-nums">
              {gameInfo?.enemyEx ?? 0}
            </CardTitle>
          </CardHeader>
        </Card>
      </div>
    </div>
  );
}
