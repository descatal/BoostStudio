import { Header } from "@/components/layout/header";
import { Link } from "@tanstack/react-router";
import { Search } from "@/components/search";
import { ThemeSwitch } from "@/components/theme-switch";
import { useAppContext } from "@/providers/app-store-provider";

const Topbar = () => {
  const topbarLinks = useAppContext((state) => state.topbarLinks);
  const topbarShowSearch = useAppContext((state) => state.topbarShowSearch);
  const topbarShowTheme = useAppContext((state) => state.topbarShowTheme);

  return (
    <Header>
      {topbarLinks.map((linkProps) => (
        <Link
          key={linkProps.path}
          className={`text-muted-foreground text-sm font-medium transition-colors hover:text-primary`}
          // @ts-ignore
          to={`/${linkProps.path}`}
          activeProps={{ className: "text-primary" }}
        >
          {linkProps.label}
        </Link>
      ))}
      <div className="ml-auto flex items-center space-x-4">
        {topbarShowSearch && <Search />}
        {topbarShowTheme && <ThemeSwitch />}
      </div>
    </Header>
  );
};

export default Topbar;
