using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
namespace project
{
    class Program
    {
        private static List<List<Node>> graph = new List<List<Node>>();
        private static Dictionary<int, List<String>> Ids_words = new Dictionary<int, List<string>>();//first_map_function
        private static Dictionary<string, int> Words_id = new Dictionary<string, int>();//second_map_function
        private static List<string> Words;
        private static int num_of_nodes = 25;
        private static int oo = 1000000;
        private static string[] split = new string[10000];
        private static int f_ans = 0;
        private static Node lca;
        public static int lca_dist(Node A, Node B, List<List<Node>> graph) //O(E+V)
        {
            int[] colors = Enumerable.Repeat(-1, num_of_nodes + 1).ToArray();
            int[] dist = Enumerable.Repeat(oo, num_of_nodes).ToArray();// intialize the array with oo 
            int[] dist1 = Enumerable.Repeat(oo, num_of_nodes + 1).ToArray();
            if (A != null && B != null)
            {
                Queue<Node> q = new Queue<Node>();
                A.node_color = 1;
                q.Enqueue(A);
                colors[A.node_value] = 1;
                dist[A.node_value] = 0;
                while (q.Count > 0)
                {
                    Node current = q.Dequeue();

                    if (current == null)
                        continue;
                    int i = 0;
                    foreach (Node cc in graph[current.node_value])
                    {
                        if (dist[cc.node_value] == oo)
                        {
                            if (cc.node_value == B.node_value)
                                colors[B.node_value] = 1;
                            dist[cc.node_value] = dist[current.node_value] + 1;
                            graph[current.node_value][i].node_color = 1;
                            //                           Console.WriteLine(graph[current.node_value][i].node_value);
                            colors[graph[current.node_value][i].node_value] = 1;
                            //                         Console.WriteLine(colors[5]);

                            q.Enqueue(cc);
                        }
                        i++;
                    }
                }// begin red
            }
            int ans = oo;
            dist1[B.node_value] = 0;
            if (colors[B.node_value] == 1)
            {
                B.node_color = 0;
                colors[B.node_value] = 0;
                lca = B;
                ans = dist1[B.node_value] + dist[B.node_value];
            }
            Queue<Node> q2 = new Queue<Node>();
            q2.Enqueue(B);
            B.node_color = 0;
            while (q2.Count > 0)
            {
                Node current1 = q2.Dequeue();
                if (current1 == null)
                    continue;
                int j = 0;
                foreach (Node cc in graph[current1.node_value])
                {

                    if (dist1[cc.node_value] == oo)
                    {
                        dist1[cc.node_value] = dist1[current1.node_value] + 1;

                        //                       Console.WriteLine(cc.node_color+"   "+cc.node_value);
                        if (colors[cc.node_value] == 1)
                        {
                            graph[current1.node_value][j].node_color = 0;// make red
                            if (dist[graph[current1.node_value][j].node_value] + dist1[graph[current1.node_value][j].node_value] < ans)
                            {
                                lca = graph[current1.node_value][j];
                                ans = dist[graph[current1.node_value][j].node_value] + dist1[graph[current1.node_value][j].node_value];
                            }
                        }
                        else
                        {
                            graph[current1.node_value][j].node_color = 0;// make red
                            colors[graph[current1.node_value][j].node_value] = 0;
                        }
                    }
                    q2.Enqueue(cc);
                    j++;
                }
            }
            f_ans = ans;
            return f_ans;
        }//lca;
        private static void Read_graphFile(string path)
        {

            if (!File.Exists(path))
                return;
            string[] lines;
            lines = System.IO.File.ReadAllLines(path);
            int w = lines.Length;
            //num_of_nodes = w + 1;
            char[] del = new char[] { ',' };
            string[] nodes;
            int z = 0, d = 0;
            Node asi = new Node();
            List<Node> li = new List<Node>();
            for (int i = 0; i < w; i++)
            {
                graph.Add(new List<Node>());
            }

            for (int i = 0; i < w; i++)
            {

                nodes = lines[i].Split(del, StringSplitOptions.RemoveEmptyEntries);

                z = int.Parse(nodes[0]);
                for (int j = 1; j < nodes.Length; j++)
                {

                    d = int.Parse(nodes[j]);
                    asi = new Node();
                    asi.node_value = d;
                    graph[z].Add(asi);

                }



            }
        }
        private static void Read_mapFile(string path)
        {
            if (!File.Exists(path))
                return;
            string[] lines;
            lines = System.IO.File.ReadAllLines(path);
            int w = lines.Length;
            char[] del = new char[] { ',' };
            string[] words;
            int Id = 0;
            List<string> li = new List<string>();
            for (int i = 0; i < w; i++)
            {
                words = lines[i].Split(del, StringSplitOptions.RemoveEmptyEntries);

                Id = int.Parse(words[0]);

                for (int j = 1; j < words.Length; j++)
                {
                    Words_id.Add(words[j], Id);
                    li.Add(words[j]);
                }
                Ids_words.Add(Id, li);
                li = new List<string>();
            }


        }
        private static void Get_words(int Id)
        {
            if (Ids_words.ContainsKey(Id))
            {
                Words = Ids_words[Id];
                return;
            }
            Words_id = null;

        }// two mapping functionso(1)
        private static int Get_id(string word)
        {
            if (Words_id.ContainsKey(word))
            {
                return Words_id[word];
            }
            return -1;
        }
        private static void readinput_file(string path)
        {
            if (!File.Exists(path))
                return;
            string[] lines, input_ids;
            lines = System.IO.File.ReadAllLines(path);
            int test_cases = int.Parse(lines[0]);
            char[] del = new char[] { ',' };
            Node A;
            Node B;
            int s, d;
            for (int i = 1; i < lines.Length; i++)
            {
                input_ids = lines[i].Split(del, StringSplitOptions.RemoveEmptyEntries);
                s = Get_id(input_ids[0]);
                d = Get_id(input_ids[1]);
                //Console.WriteLine(s+"   "+d);
                A = new Node();
                A.node_value = s;
                B = new Node();
                B.node_value = d;
                if (A != null && B != null)
                {
                    int u = lca_dist(A, B, graph);
                    Get_words(lca.node_value);
                    Console.WriteLine(input_ids[0] + "," + input_ids[1] + "     " + u + " SCA " + Words[0]);
                }

            }
        }
        private static string removewhite(string input)
        {
            int j = 0, inputlen = input.Length;
            char[] newarr = new char[inputlen];
            for (int i = 0; i < inputlen; i++)
            {
                char tmp = input[i];
                if (!char.IsWhiteSpace(tmp))
                {
                    newarr[j] = tmp;
                    ++j;

                }
            }
            return new string(newarr, 0, j);
        }
        private static void Readinput2(string path)
        {
            List<string> outcast = new List<string>();
            int min = -100000;
            if (!File.Exists(path))
                return;
            string[] lines;
            lines = System.IO.File.ReadAllLines(path);
            int test_cases = int.Parse(lines[0]);
            char[] del = new char[] { ',' };
            string[] arr;
            int r;
            Node A, B;
            int sum = 0, h = 0;
            int ans = 0;
            List<List<int>> li = new List<List<int>>();
            List<int> l2;
            for (int i = 1; i < lines.Length; i++)
            {
                arr = lines[i].Split(del, StringSplitOptions.RemoveEmptyEntries);
                l2 = new List<int>();

                for (int j = 0; j < arr.Length; j++)
                {
                    if (j + 1 == arr.Length)
                    {
                        arr[j] = removewhite(arr[j]);
                        r = Get_id(arr[j]);
                    }
                    else
                        r = Get_id(arr[j]);
                    l2.Add(r);
                }
                li.Add(l2);
            }

            for (int i = 0; i < lines.Length - 1; i++)
            {
                sum = 0; min = -100000; ans = 0;
                for (int j = 0; j < li[i].Count; j++)
                {
                    A = new Node();
                    A.node_value = li[i][j];
                    for (int k = 0; k < li[i].Count; k++)
                    {
                        B = new Node();
                        B.node_value = li[i][k];

                        h = lca_dist(A, B, graph);
                        // Console.WriteLine(A.node_value + " " + B.node_value + " " + h);
                        sum += h;
                    }
                    if (sum >= min)
                    {
                        min = sum;
                        ans = li[i][j];
                    }
                    sum = 0;
                }
                Get_words(ans);
                outcast.Add(Words[0]);
            }
            int lineindx = 1;
            foreach (string a in outcast)
            {
                Console.WriteLine(lines[lineindx++] + "      " + "outcast=" + a);

            }

        }
        static void Main(string[] args)
        {
            Node ee1=new Node();
            Node ee2=new Node();
            ee1.node_value = 1;
            ee2.node_value = 1;
            string graph_filepath = "graph4.txt";
            string map_filepath = "map4.txt";
            string input1 = "input4.txt";
           // string input2 = "input11.txt";
            Read_graphFile(graph_filepath);
          Read_mapFile(map_filepath);
            readinput_file(input1);
            Console.WriteLine();
            Get_words(10);
            Console.WriteLine(Words[0]);
          //  Readinput2(input1);
        }
    }
}
