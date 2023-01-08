import ReactMarkdown from 'react-markdown';
import readingTime from 'reading-time';

interface BlogProps {
    title: string;
    summary: string;
}

const mockMD = `
## Illa poscere arboris armatur levatus

Lorem markdownum sumpto natasque caelo palluit postes et vocis semper, amor.
Nuper fuit vitam, sed non causa spatio bicorni quae virtutem quoque sustinet
sparsas alternae se somni. Matri retinacula Cypriae carinam: protinus tumulo!
Nulloque tabellae Phoebum ea mentita
[tollit](http://et-anhelatos.org/raucis-ager.html) adire; et hac nam Philomela
spoliat iungit, manu. Genetrix interea, via hanc moveri: iuga novitate,
Cepheaque nec Phineus, puellis.

## Deceat femina laudat ora dentibus

Cremarat volventia capillis titubare vacuam Cerberei habet concubitusque utere
sua currus quoque sanus. Nisi laesi propositique iaculum tendentemque primo
Iovis foliis perdidimus de novem fluvialis.

1. Aut est
2. Procumbere Ioles granum per hominumque gaudet matrisque
3. Guttura quaque
4. Murmur armis illo raptos sedere

## Lacerto vacuus tutior afuerunt flectique bracchia cetera

Amori cum secuta taedae et magno, non manusque nocte operisque sinus, a? Capere
timor, quicquam frena, quia vale undis Dolopum *laturus* adstitit eburnea?

- Alta tellus brumalis modo nec declivibus
- Quacumque freta
- Qua ut huic deducere favent pater

## Mater duarum maturuit rogant terraeque in oblivia

Hymettia usum, **est parum teque** victor! Ore vides factique aeternum bella huc
adnuerant; vasti calido. Tristia et et voce habenti invenio cunctisque pudori
perque sibi concurrere esse, cortinaque visus famularia ova. Aer moriens
**pariter eandem sonitum** pellis inquit, naturale coniugis **discutit** Phinea,
canum gulae crede praereptaque.

## Omnis Iove eodem illo his hoste

Meruique si Solem patriae fertur superest generatus siccare, est
[quod](http://ut.net/nostrisanimal.aspx) Solem pulcherrima, caede; caecos vivit
per. *Flavescere* venti vix domuit, fratres Hypsipyles Amyntor mea fata. Litora
nescio quamvis: pavidum oculos.

Veni vale cessit mons, mea ipse maturus receptae deos non caelum tam aries
gemitumque altera. Boves dum, genitoris vincite, vade tectura **munera
dominum**. Consistite pignus.
`

export default function Blog(props: BlogProps) {

    return (
        <div className="m-4 p-8 grid grid-cols-1 gap-4 lg:w-1/2 mx-auto">
            <div className="flex">
                <div className="avatar">
                    <div className="w-12 rounded-full">
                        <img src="https://placeimg.com/192/192/people" />
                    </div>
                </div>
                <div className="ml-4">
                    <p className="font-semibold">Autor Autorescu</p>
                    <div>
                        <p className="inline font-thin text-sm align-text-bottom">21 January 2023</p>
                        <p className="inline font-bold tracking-widest ml-2">·</p>
                        <p className="inline font-thin text-sm align-text-bottom ml-2">{readingTime(mockMD).text}</p>
                    </div>
                </div>
            </div>

            <h1 className="text-4xl font-bold">Lorem ipsum</h1>
            <p className="text-lg">Lorem lorem lorem lorem lorem lorem lorem</p>
            <img className="mx-auto w-full" src="https://picsum.photos/800/400" />
            <div>
                <ReactMarkdown className='leading-relaxed space-y-4' children={mockMD} components={{
                    h2({ children }) {
                        return (<div className='space-y-0'><p className='text-2xl font-semibold'>{children}</p><div className="divider"></div></div>);
                    },
                    ul({ children }) {
                        return <ul className="list-disc list-inside indent-4">{children}</ul>
                    },
                    ol({ children }) {
                        return <ol className="list-decimal list-inside indent-4">{children}</ol>
                    },
                    strong({ children }) {
                        return <strong className='font-bold decoration-solid decoration-accent underline'>{children}</strong>
                    },
                    a({ children, href, title }) {
                        return <a className='link link-accent' href={href} title={title}>{children}</a>
                    },
                    em({ children }) {
                        return <em className='text-accent font-italic'>{children}</em>
                    }
                }} />
            </div>
        </div>
    );
}