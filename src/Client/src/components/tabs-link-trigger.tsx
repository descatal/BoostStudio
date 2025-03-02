import React from "react";
import { Link } from "@tanstack/react-router";
import { TabsTrigger } from "@/components/ui/tabs";

export const TabsLinkTrigger: React.FC<{
  href: string;
  children: React.ReactNode;
}> = ({ href, children }) => (
  <TabsTrigger value={href} asChild>
    <Link to={href}>{children}</Link>
  </TabsTrigger>
);
