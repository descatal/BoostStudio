import React from "react";

import { cn } from "@/lib/utils";
import { Separator } from "@/components/ui/separator";
import { Button } from "@/components/ui/button";
import { PanelLeft } from "lucide-react";

interface HeaderProps extends React.HTMLAttributes<HTMLElement> {
  fixed?: boolean;
  ref?: React.Ref<HTMLElement>;
}

export const Header = ({
  className,
  fixed,
  children,
  ...props
}: HeaderProps) => {
  const [offset, setOffset] = React.useState(0);

  React.useEffect(() => {
    const onScroll = () => {
      setOffset(document.body.scrollTop || document.documentElement.scrollTop);
    };

    // Add scroll listener to the body
    document.addEventListener("scroll", onScroll, { passive: true });

    // Clean up the event listener on unmount
    return () => document.removeEventListener("scroll", onScroll);
  }, []);

  return (
    <header
      className={cn(
        "flex h-16 items-center gap-3 bg-background p-4 sm:gap-4",
        fixed && "header-fixed peer/header fixed z-50 w-[inherit] rounded-md",
        offset > 10 && fixed ? "shadow" : "shadow-none",
        className,
      )}
      {...props}
    >
      <Button
        data-sidebar="trigger"
        variant="ghost"
        size="icon"
        className={cn("h-7 w-7", className)}
      >
        <PanelLeft />
        <span className="sr-only">Toggle Sidebar</span>
      </Button>
      <Separator orientation="vertical" className="h-6" />
      {children}
    </header>
  );
};

Header.displayName = "Header";
