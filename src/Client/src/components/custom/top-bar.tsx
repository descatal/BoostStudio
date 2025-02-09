import React from 'react';
import {cn} from "@/lib/utils"

const TopBar = (
  {
    children,
    className,
  }: {
    children: React.ReactNode,
    className?: string | undefined,
  }) => {
  return (
    <div className={cn("sticky top-0 z-10 border-b flex h-16 items-center p-10 bg-background/80 backdrop-blur-sm", className)}>
      {children}
    </div>
  );
};

export default TopBar;