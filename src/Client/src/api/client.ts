import { CreateClientConfig } from "@/api/exvs/client.gen";

export const createClientConfig: CreateClientConfig = (config) => ({
  ...config,
  baseUrl: import.meta.env.VITE_SERVER_URL ?? "",
});
