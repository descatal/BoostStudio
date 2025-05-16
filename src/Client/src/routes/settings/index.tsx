import { createFileRoute } from "@tanstack/react-router";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card.tsx";
import ConfigsList from "@/features/configs/components/configs-list.tsx";

export const Route = createFileRoute("/settings/")({
  component: RouteComponent,
});

const configs = [
  {
    label: "Staging Directory",
    key: "STAGING_DIRECTORY",
    description: "The `.moddedboost` folder inside your RPCS3 directory.",
    defaultValue: "/rpcs3/.moddedboost",
  },
  {
    label: "Working Directory",
    key: "WORKING_DIRECTORY",
    description: "The working directory to store intermediate asset files.",
    defaultValue: "/workstation",
  },
  {
    label: "Production Directory",
    key: "PRODUCTION_DIRECTORY",
    description: "The directory that stores packed psarc files.",
    defaultValue: "/rpcs3/dev_hdd0/game/NPJB00512/USRDIR",
  },
  {
    label: "Script Directory",
    key: "SCRIPT_DIRECTORY",
    description: "The directory that stores units' scex script files.",
    defaultValue: "/scripts",
  },
];

function RouteComponent() {
  return (
    <div className={"flex justify-center w-full"}>
      <Card className={"m-8 w-4xl"}>
        <CardHeader>
          <CardTitle className={"text-2xl"}>Settings</CardTitle>
          <CardDescription className={"text-lg"}>
            Configure your app settings.
          </CardDescription>
        </CardHeader>
        <CardContent>
          <ConfigsList
            className={"flex w-full min-w-0 flex-col gap-6"}
            configOptions={configs}
          />
        </CardContent>
      </Card>
    </div>
  );
}
