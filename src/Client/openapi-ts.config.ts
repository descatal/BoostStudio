import { defaultPlugins } from "@hey-api/openapi-ts";

export default {
  input: "src/api/booststudio_exvs.json",
  output: "src/api/exvs",
  plugins: [
    ...defaultPlugins,
    {
      name: "@hey-api/client-fetch",
      runtimeConfigPath: "./src/api/client.ts",
    },
    "@tanstack/react-query",
    "zod",
  ],
};
