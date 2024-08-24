import { useEffect, useState } from "react";

interface Props {}

type Themes = "theme-light" | "theme-dark";

const themes: Record<Themes, string> = {
  "theme-light": "#f5f6f9",
  "theme-dark": "#222",
};

const ThemeSwitcher = (props: Props) => {
  const [selectedTheme, setSelectedTheme] = useState<Themes>("theme-light");

  const setTheme = (theme: Themes) => () => {
    setSelectedTheme(theme);
    localStorage.setItem("theme", theme);
  };

  useEffect(() => {
    setSelectedTheme(
      (localStorage.getItem("theme") as Themes) || "theme-light"
    );
  }, []);

  useEffect(() => {
    const main = document.getElementById("main");
    if (main) {
      main.className = main.className.replace(/theme-\w+/, selectedTheme);
    }
  }, [selectedTheme]);

  return (
    <div className="flex items-center mr-8">
      {Object.entries(themes).map(([theme, color]) => (
        <button
          key={theme}
          onClick={setTheme(theme as Themes)}
          className={
            "rounded-full w-4 h-4 bg-default-color border mr-2 focus:outline-none" +
            (selectedTheme === theme ? " border-accent" : "")
          }
          style={{ backgroundColor: color }}
        ></button>
      ))}
    </div>
  );
};

export default ThemeSwitcher;
