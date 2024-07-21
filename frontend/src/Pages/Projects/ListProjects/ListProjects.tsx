import { useEffect, useState } from "react";
import { Project } from "../../../birdboard";
import { Link } from "react-router-dom";
import ProjectCard from "../../../Components/Projects/ProjectCard/ProjectCard";

interface Props {}

const ListProjects = (props: Props) => {
  const [projects, setProjects] = useState<Project[]>([]);
  useEffect(() => {
    setProjects([
      { title: "Title", description: "description", path: "/projects/1" },
      { title: "Title", description: "description", path: "/projects/2" },
      { title: "Title", description: "description", path: "/projects/2" },
      { title: "Title", description: "description", path: "/projects/2" },
      {
        title: "Title",
        description: `Lorem ipsum dolor sit amet consectetur adipisicing elit. Explicabo totam, nam blanditiis magni sint eveniet illo, fuga, earum pariatur reiciendis eaque architecto officia sit accusantium iste eligendi iusto repellendus maiores.
  Voluptate, voluptatem. Possimus voluptatibus provident optio dolore sequi, sunt, iure consequuntur in laboriosam minima est dolorem nesciunt laborum accusantium quia placeat! Explicabo ipsum quae ipsam consequuntur voluptatibus praesentium, natus blanditiis?
  Provident tempore voluptatibus pariatur molestias alias ratione! Maiores corporis quam reprehenderit consectetur nesciunt suscipit laboriosam voluptates hic! Eum tenetur quaerat, illo iusto laudantium in cupiditate dicta accusamus maxime? Sit, velit.
  Beatae aliquid adipisci numquam asperiores tenetur. Dignissimos, delectus rerum maiores, rem voluptatem quam totam dolorem dolores labore laboriosam nulla illum cupiditate hic. Iste consequatur necessitatibus consequuntur ad cum dolore qui!
  Eos, ad? Voluptates alias, ad modi veritatis dolor perferendis. Tempore maiores cumque distinctio illum autem? Nobis, obcaecati ex veritatis illo quidem doloribus sint neque, tempore odio impedit maxime debitis minima!
  Facere quas, voluptatum dignissimos ipsum reiciendis nulla maiores, a exercitationem officiis pariatur temporibus ex autem libero nisi animi iusto molestias rem aspernatur! Voluptate ex quas ea possimus enim libero cumque.
  Fugiat consequuntur ad nisi id eius tempora natus sint libero. Impedit incidunt atque cumque possimus ad ratione architecto, vitae unde eos, dolores vel veritatis deserunt quia? Adipisci perspiciatis dolores odit.
  Voluptas, sed consectetur sit ex asperiores error inventore quos culpa. Mollitia deleniti, amet neque sed sapiente porro numquam laboriosam fugiat autem eaque adipisci, non explicabo nisi odio quidem ratione ad.
  Repellat placeat, quod at alias asperiores corrupti sunt quam impedit. Ratione sed quibusdam ipsa. Ullam cum reprehenderit voluptate aperiam odio molestias asperiores exercitationem distinctio ratione. Odio ullam voluptates eos explicabo?
  Vero sint sed id facilis, doloremque libero nobis obcaecati optio consequuntur odio sequi quidem architecto explicabo esse dolore. Quaerat incidunt assumenda reprehenderit, temporibus dignissimos deleniti laudantium totam perspiciatis cumque ratione.
  Consectetur temporibus omnis perspiciatis, iste nobis reprehenderit sint beatae esse atque commodi asperiores aliquam autem, magni cupiditate debitis, qui fugit eos vel quod doloribus eligendi facere. Velit cum praesentium corporis!
  Corrupti natus quas beatae, est, excepturi quibusdam saepe officia voluptate velit laborum impedit culpa iusto! Reiciendis eum molestias voluptates laborum quisquam alias maxime, vitae sequi obcaecati cum cumque illum quia.
  Consectetur reprehenderit est natus fugiat laboriosam facilis dolorum aliquam dolor esse deleniti eveniet minus sequi explicabo, ullam atque perferendis labore eum unde sit nam praesentium nesciunt beatae eaque repellendus. Excepturi.
  Tempora amet odit nobis incidunt qui ad. Veritatis ullam numquam expedita hic ex doloremque impedit laborum rerum non quis fuga autem ipsum eligendi quisquam cumque odit, ab, cupiditate qui odio!
  Illum ullam et animi libero dolorum. Pariatur, placeat. Assumenda nam quis vel repudiandae veritatis eum aspernatur expedita tempora. Optio rerum repudiandae fugiat molestiae veritatis eaque, quibusdam eius delectus maxime qui!
  Excepturi laboriosam aliquam rerum esse at officiis praesentium. Modi aspernatur numquam minima aliquid quaerat dolor dolores perspiciatis eligendi enim impedit eum eius unde animi nam dicta voluptates consequuntur, mollitia optio.
  Magnam, at? Distinctio commodi neque et deleniti hic repudiandae quos error, eum temporibus dignissimos exercitationem iusto pariatur non earum velit id praesentium qui quas illo veniam fuga, sunt esse dolores.`,
        path: "/  projects/3",
      },
    ]);
  }, []);

  return (
    <>
      <header className="flex items-center mb-3 py-4">
        <div className="flex justify-between items-end w-full">
          <h2 className="text-grey text-sm font-normal">My Projects</h2>
          <Link className="button" to="/projects/create">
            New Project
          </Link>
        </div>
      </header>

      <main className="lg:flex lg:flex-wrap -mx-3">
        {projects ? (
          projects.map((project) => (
            <div className="lg:w-1/3 px-3 pb-6">
              <ProjectCard project={project} />
            </div>
          ))
        ) : (
          <div>No projects yet.</div>
        )}
      </main>
    </>
  );
};

export default ListProjects;
