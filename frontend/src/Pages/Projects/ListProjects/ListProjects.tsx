import React, { useEffect, useState } from "react";
import { Project } from "../../../birdboard";
import { Link } from "react-router-dom";
import { stringLimit } from "../../../Helpers/StringLimit";

interface Props {}

const ListProjects = (props: Props) => {
  const [projects, setProjects] = useState<Project[]>([]);
  useEffect(() => {
    setProjects([
      { title: "Title", description: "description", path: "/projects/1" },
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
  Magnam, at? Distinctio commodi neque et deleniti hic repudiandae quos error, eum temporibus dignissimos exercitationem iusto pariatur non earum velit id praesentium qui quas illo veniam fuga, sunt esse dolores.
  Soluta ipsam illo aspernatur tenetur harum reiciendis aperiam, repellendus ex inventore delectus eveniet molestiae atque maxime asperiores labore iusto nemo cum nulla error sunt suscipit recusandae dolorem. Nesciunt, quasi expedita!
  Commodi, in. Veniam excepturi ut iure odio! Perferendis earum, atque dolore expedita qui perspiciatis molestiae veritatis maxime esse doloremque ipsa, velit iure ex, deleniti totam maiores enim est officia dignissimos!
  Impedit consectetur maiores cupiditate dolorem id cum placeat tenetur blanditiis hic? Quidem fugiat laboriosam sed accusantium, eligendi harum quasi veritatis dolor. Magnam ullam aliquid debitis saepe facilis rerum, maiores voluptatum!
  Corporis architecto saepe dolor ad! Eum quis magnam eveniet aliquid consectetur quia recusandae perferendis ipsum odio ratione harum numquam, commodi excepturi modi minima, tenetur sit. Blanditiis quam odio deleniti porro?
  Dolores odio quia, consequuntur unde repellat architecto a ipsa praesentium nobis blanditiis! Incidunt illum a, quis exercitationem id aliquam? Maiores ut nihil obcaecati atque cumque reprehenderit odit ipsa totam eos.
  Nesciunt, provident at! Minus quidem placeat excepturi eos at amet eligendi impedit, reprehenderit pariatur deleniti nostrum vitae cupiditate blanditiis sint dolor? Pariatur, tempore vel. Nobis magnam beatae voluptatem corrupti culpa.
  Dolorem voluptatum praesentium ut nobis doloribus ex dicta ducimus? Accusamus facilis eligendi natus est accusantium amet rerum esse voluptate quo, ipsum, cum expedita animi! Minima explicabo autem voluptas mollitia officiis.
  Natus quia, vitae tempora fugit vel vero soluta perferendis consectetur quas recusandae ea error voluptatum eius itaque qui iste facilis nesciunt libero commodi aut sunt corporis delectus quam. Quia, dolores?
  Ullam at corporis, amet quos, sequi alias non nihil necessitatibus commodi aperiam adipisci dicta totam laboriosam quaerat labore magni qui culpa veniam quam laudantium? Quod totam impedit autem adipisci dolores.
  Alias eveniet ratione repellat voluptas unde dicta, temporibus deleniti. Blanditiis odit dignissimos culpa veniam excepturi numquam dolor consequatur, doloremque unde soluta, laborum necessitatibus, provident quis aliquid iusto. Officia, sint alias.
  Asperiores possimus iste esse consectetur neque quos quasi porro adipisci quo laudantium itaque culpa eaque enim voluptate, nihil, deleniti accusamus autem, quia cum perferendis nostrum. Placeat cupiditate blanditiis eaque assumenda!
  Delectus rem optio dolores corporis maiores quis voluptas amet doloribus ea, quasi expedita enim laborum sunt iste nam quae voluptatibus dicta, voluptatum eos, facilis earum est perspiciatis? Dolore, odit provident.
  Eligendi sit rerum consequuntur id enim, laboriosam ipsum dolor nisi. Illo omnis, excepturi neque porro nulla accusantium sapiente. Nulla quos tempore impedit? Voluptatum nam nobis tenetur consectetur fugiat ex laudantium.
  Ab voluptates quisquam perspiciatis in asperiores ea accusamus inventore rerum necessitatibus incidunt nam sunt, unde illo dolor. Consectetur, aut ratione nesciunt voluptatibus temporibus dolor doloribus consequuntur suscipit. Quidem, officia incidunt?
  Similique, enim debitis! Harum laboriosam ad tempora nemo impedit magnam dignissimos, cumque delectus vero quas placeat perspiciatis, qui dolorum voluptatibus illum adipisci. Dolorum quasi itaque voluptatum possimus enim? Quasi, accusamus.
  Eaque ad unde repellat quisquam, repellendus ullam amet aliquid provident rerum neque! Distinctio mollitia est eos sunt! Accusamus perferendis quos, iure, fugiat necessitatibus sunt ut inventore quidem facilis expedita enim?
  Cupiditate facilis pariatur molestiae assumenda deleniti. Omnis officia ab recusandae placeat, vero laudantium nisi eius eligendi, eum alias eaque vitae soluta aliquam animi deserunt qui cupiditate excepturi quisquam. Sed, ipsa.
  Doloremque corporis illo obcaecati temporibus amet laboriosam dignissimos facilis, aperiam quas veritatis sequi illum ex vitae culpa, cupiditate a, consequuntur quam iusto dicta. Laudantium dicta, eveniet ab alias nemo hic?
  Odio laborum sit cupiditate, facilis repellendus labore possimus cum porro non. Rem aspernatur architecto amet nobis beatae nihil id laboriosam nostrum, eligendi ut labore ad ea voluptatum. Minima, adipisci quisquam!
  Enim expedita, architecto corporis sit ea voluptatum, facere impedit quasi quod accusantium animi laboriosam odio consequatur harum ducimus blanditiis placeat eveniet! Voluptates sit, commodi ex cumque sint quis provident ab?
  Eos, magnam vero expedita nesciunt quis incidunt. Architecto amet cupiditate suscipit possimus, repellendus asperiores sint nisi molestiae, dolorum autem temporibus commodi ipsa. Enim praesentium quasi doloremque repudiandae quidem ex commodi.
  Assumenda ipsa, necessitatibus delectus illo architecto adipisci! Inventore earum cupiditate dignissimos quam alias dicta ducimus magni veniam pariatur? Officia, cumque! Ducimus maxime libero pariatur iste dicta quae corrupti consequuntur optio?
  Officia sed est corporis inventore eius placeat. Molestiae aperiam optio accusantium odit maxime ut magni unde dolorum? Numquam possimus similique corporis itaque illo enim? Aspernatur laudantium officia ducimus nemo earum.
  Hic assumenda repellendus impedit omnis aspernatur quas accusantium, dolorum rerum similique, ex quo magnam velit. Aut asperiores dignissimos iure enim aliquam laboriosam perferendis. Voluptates repellendus eos consequatur nam, alias repudiandae.
  Repellat voluptas quasi earum nesciunt. Labore, sint laudantium accusamus magnam voluptatibus pariatur, fugit eligendi aperiam iste rem numquam neque deserunt voluptates error! Temporibus, nihil suscipit! Corrupti voluptates dignissimos eos! Officiis!
  Nisi ducimus, natus recusandae adipisci quam placeat doloribus, asperiores, repellat provident nostrum reiciendis temporibus similique culpa aliquid accusamus totam! Praesentium ullam, non placeat accusamus totam est fugit neque velit modi.
  Ipsum maxime, enim laboriosam saepe, amet iste sed quam excepturi expedita hic nesciunt molestiae debitis ex, id dolor facere. Accusantium earum tempore non sunt doloremque libero sed dolores, vero eum.
  Eos officiis molestias laudantium? Sapiente perferendis quisquam laborum repellendus eveniet et culpa voluptas, quis aliquid hic accusamus quasi, tempora repudiandae deserunt distinctio? Dolores nesciunt placeat eligendi, neque pariatur tempore iure?
  Dolore natus quisquam, architecto veritatis accusamus quia dolorem sunt dicta corporis! Voluptas maiores nostrum laborum impedit veritatis eius eum aspernatur repudiandae soluta, blanditiis totam, magni veniam possimus exercitationem dolores molestias.
  Incidunt consectetur consequuntur neque ea officia. Minima sunt itaque tenetur omnis, adipisci eligendi vel at praesentium maxime labore optio. Dolorem culpa odio possimus libero! Laudantium asperiores expedita earum accusantium animi?
  Voluptatibus eligendi non ea quam laborum provident consectetur ex! Non iusto quod molestiae corporis ipsam nisi, quae, excepturi perspiciatis vitae magnam recusandae odit in porro ex at impedit officiis repellendus.
  Dolorum commodi deleniti quis, suscipit culpa nobis nostrum, qui voluptatibus repellat repudiandae dicta minus laborum molestiae eum explicabo quas voluptatum vero quae voluptates maxime nihil dolores iste consequatur tempora. Omnis!
  Cupiditate tempore voluptate maxime corporis, eum aliquid doloremque laboriosam accusamus voluptatibus mollitia possimus temporibus, quis atque magnam. Hic voluptates corrupti sed architecto, voluptatem mollitia reprehenderit officia, quo adipisci, veniam vero.`,
        path: "/  projects/3",
      },
    ]);
  }, []);

  return (
    <>
      <div className="flex items-center mb-3">
        <Link className="text-xl font-semibold" to="/projects/create">
          New Project
        </Link>
      </div>

      <div className="flex">
        {projects ? (
          projects.map((project) => (
            <Link
              className="bg-white mr-4 p-5 rounded shadow w-1/3"
              style={{ height: "200px" }}
              to={project.path}
            >
              <h3 className="font-normal text-xl py-4">{project.title}</h3>
              <div className="text-grey">
                {stringLimit(project.description, 100)}
              </div>
            </Link>
          ))
        ) : (
          <div>No projects yet.</div>
        )}
      </div>
    </>
  );
};

export default ListProjects;
