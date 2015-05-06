using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CB
{
    class CloudSearch
    {
        protected Dictionary<string, Object> dictionary = new Dictionary<string, Object>();
        public CloudSearch(string tableName)
        {
            dictionary["collectionNames"] = tableName;
            dictionary["query"] = new Dictionary<string, Object>();
            dictionary["from"] = 0;
            dictionary["size"] = 10;
            dictionary["sort"] = new ArrayList();
        }

        public CloudSearch(ArrayList tableNames)
        {
            dictionary["collectionNames"] = tableNames;
            dictionary["query"] = new Dictionary<string, Object>();
            dictionary["from"] = 0;
            dictionary["size"] = 10;
            dictionary["sort"] = new ArrayList();
        }

        public int Skip
        {
            get
            {
                return (int)dictionary["from"];
            }
            set
            {
                dictionary["from"] = value;
            }
        }

        public int Limit
        {
            get
            {
                return (int)dictionary["size"];
            }
            set
            {
                dictionary["size"] = value;
            }
        }

        public CloudSearch OrderByAsc(string column)
        {
            Dictionary<string, Object> temp = new Dictionary<string, object>();
            temp[column] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)temp[column])["order"] = "asc";
            ((ArrayList)dictionary["sort"]).Add(temp);

            return this;
        }

        public CloudSearch OrderByDesc(string column)
        {
            Dictionary<string, Object> temp = new Dictionary<string, object>();
            temp[column] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)temp[column])["order"] = "desc";
            ((ArrayList)dictionary["sort"]).Add(temp);

            return this;
        }

        public CloudSearch NotEqualTo(string column, Object data)
        {
            //data can bean array too!
            var term = new Dictionary<string, Object>();

            if (data.GetType() == typeof(ArrayList))
            {
                term["terms"] = new Dictionary<string, Object>();
                ((Dictionary<string, Object>)term["terms"])[column] = data;
            }
            else
            {
                term["term"] = new Dictionary<string, Object>();
                ((Dictionary<string, Object>)term["term"])[column] = data;
            }

            this._PushInMustNotFilter(term);

            return this;
        }

        public CloudSearch EqualTo(string column, Object data)
        {
            //data can bean array too!
            var term = new Dictionary<string, Object>();

            if (data.GetType() == typeof(ArrayList))
            {
                term["terms"] = new Dictionary<string, Object>();
                ((Dictionary<string, Object>)term["terms"])[column] = data;
            }
            else
            {
                term["term"] = new Dictionary<string, Object>();
                ((Dictionary<string, Object>)term["term"])[column] = data;
            }

            this._PushInMustFilter(term);

            return this;
        }

        public CloudSearch Exists(string column)
        {
            //data can bean array too!
            var obj = new Dictionary<string, Object>();
            obj["exists"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)obj["exists"])["field"] = column;

            this._PushInMustFilter(obj);

            return this;
        }

        public CloudSearch DoesNotExist(string column)
        {
            //data can bean array too!
            var obj = new Dictionary<string, Object>();
            obj["missing"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)obj["missing"])["field"] = column;

            this._PushInMustFilter(obj);

            return this;
        }

        public CloudSearch GreaterThanOrEqual(string column, Object data)
        {
            //data can bean array too!
            var obj = new Dictionary<string, Object>();
            obj["range"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)obj["range"])[column] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)((Dictionary<string, Object>)obj["range"])[column])["gte"] = data;
            this._PushInMustFilter(obj);

            return this;
        }



        public CloudSearch GreaterThan(string column, Object data)
        {
            //data can bean array too!
            var obj = new Dictionary<string, Object>();
            obj["range"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)obj["range"])[column] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)((Dictionary<string, Object>)obj["range"])[column])["gt"] = data;
            this._PushInMustFilter(obj);

            return this;
        }

        public CloudSearch LessThan(string column, Object data)
        {
            //data can bean array too!
            var obj = new Dictionary<string, Object>();
            obj["range"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)obj["range"])[column] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)((Dictionary<string, Object>)obj["range"])[column])["lt"] = data;
            this._PushInMustFilter(obj);

            return this;
        }

        public CloudSearch LessThanOrEqual(string column, Object data)
        {
            //data can bean array too!
            var obj = new Dictionary<string, Object>();
            obj["range"] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)obj["range"])[column] = new Dictionary<string, Object>();
            ((Dictionary<string, Object>)((Dictionary<string, Object>)obj["range"])[column])["lte"] = data;
            this._PushInMustFilter(obj);

            return this;
        }



        /* PRIVATE FUNCTIONS */

        private bool _IsFilteredQuery;

        private void _PushInMustFilter(Object obj)
        {
            this._MakeFilteredQuery();

            if (((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"]) != null && ((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"])["must"] != null)
            {
                //attach this term to an array of 'must'.
                ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"])["must"]).Add(obj);
            }
            else if (this._GetFilterItem() != null)
            {
                //if I already have a exists, then :
                //create a bool and and all of these.
                this._AppendPrevFilterToBool();
                ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"])["must"]).Add(obj);

            }
            else
            {
                ((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"] = obj;
            }
        }

        private void _PushInShouldFilter(Object obj)
        {
            this._MakeFilteredQuery();

            if (((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"]) != null && ((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"])["should"] != null)
            {
                //attach this term to an array of 'must'.
                ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"])["should"]).Add(obj);
            }
            else if (this._GetFilterItem() != null)
            {
                //if I already have a exists, then :
                //create a bool and and all of these.
                this._AppendPrevFilterToBool();
                ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"])["should"]).Add(obj);

            }
            else
            {
                this._CreateBoolFilter();
                ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"])["should"]).Add(obj);
            }
        }

        private void _PushInMustNotFilter(Object obj)
        {
            this._MakeFilteredQuery();

            if (((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"]) != null && ((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"])["must_not"] != null)
            {
                //attach this term to an array of 'must'.
                ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"])["must_not"]).Add(obj);
            }
            else if (this._GetFilterItem() != null)
            {
                //if I already have a exists, then :
                //create a bool and and all of these.
                this._AppendPrevFilterToBool();
                ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"])["must_not"]).Add(obj);

            }
            else
            {
                this._CreateBoolFilter();
                ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"])["must_not"]).Add(obj);
            }
        }

        private void _MakeFilteredQuery()
        {
            if (((Dictionary<string, Object>)dictionary["query"])["filtered"] == null)
            {
                var prevQuery = (Dictionary<string, Object>)dictionary["query"];
                dictionary["query"] = new Dictionary<string, Object>();
                ((Dictionary<string, Object>)dictionary["query"])["filtered"] = new Dictionary<string, Object>();
                ((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["query"] = prevQuery;
                ((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"] = new Dictionary<string, Object>();
            }

            _IsFilteredQuery = true;
        }

        private Dictionary<string, Object> _GetFilterItem()
        {
            if (((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"]).Count > 0)
            {
                return (Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"];
            }

            return null;
        }

        private void _DeleteFilterItem()
        {
            ((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"] = new Dictionary<string, Object>();
        }

        private void _CreateBoolFilter()
        {
            if (((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"] == null)
            {
                ((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"] = new Dictionary<string, Object>();
            }

            if (((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"])["must"] == null)
            {
                ((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"])["must"] = new ArrayList();
            }

            if (((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"])["should"] == null)
            {
                ((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"])["should"] = new ArrayList();
            }

            if (((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"])["must_not"] == null)
            {
                ((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"])["must_not"] = new ArrayList();
            }
        }



        private void _CreateBoolQuery()
        {

            if (((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"] == null)
            {
                ((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"] = new Dictionary<string, Object>();
            }

            if (((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"])["must"] == null)
            {
                ((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"])["must"] = new ArrayList();
            }

            if (((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"])["should"] == null)
            {
                ((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"])["should"] = new ArrayList();
            }

            if (((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"])["must_not"] == null)
            {
                ((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"])["must_not"] = new ArrayList();
            }


            if (((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"]) == null)
            {
                if (((Dictionary<string, Object>)dictionary["query"])["bool"] == null)
                {
                    ((Dictionary<string, Object>)dictionary["query"])["bool"] = new Dictionary<string, object>();
                }

                if (((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["bool"])["must"] == null)
                {
                    ((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["bool"])["must"] = new ArrayList();
                }

                if (((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["bool"])["should"] == null)
                {
                    ((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["bool"])["should"] = new ArrayList();
                }


                if (((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["bool"])["must_not"] == null)
                {
                    ((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["bool"])["must_not"] = new ArrayList();
                }
            }
            else
            {
                if (((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["query"])["bool"] == null)
                {
                    ((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["query"])["bool"] = new Dictionary<string, Object>();
                }

                if (((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["query"])["bool"])["must"] == null)
                {
                    ((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["query"])["bool"])["must"] = new ArrayList();
                }

                if (((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["query"])["bool"])["should"] == null)
                {
                    ((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["query"])["bool"])["should"] = new ArrayList();
                }

                if (((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["query"])["bool"])["must_not"] == null)
                {
                    ((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["query"])["bool"])["must_not"] = new ArrayList();
                }
            }
        }

        private void _AppendPrevFilterToBool()
        {
            var prevTerm = this._GetFilterItem();
            this._DeleteFilterItem();
            this._CreateBoolFilter();
            ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"])["must"]).Add(prevTerm);
        }

        private void _PushInShouldQuery(Object obj)
        {
            this._MakeFilteredQuery();

            if (((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"] !=null  && ((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"])["should"]!=null) 
            {
                //attach this term to an array of 'must'.
                ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"])["should"]).Add(obj);   
            } 
            else if (this._GetFilterItem()!=null)
            {
                //if I already have a exists, then :
                //create a bool and and all of these.
                this._AppendPrevFilterToBool();
                ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"])["should"]).Add(obj);   

            }
            else
            {
                this._CreateBoolFilter();
                ((ArrayList)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["filtered"])["filter"])["bool"])["must_not"]).Add(obj);
            }
        }
        public CloudSearch SearchOn(ArrayList columns, string query)
        {
            if (this._IsFilteredQuery)
            {
                //if columns is an array.
                (((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["query"])["multi_match"]) = new Dictionary<string, object>();
                (((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["query"])["multi_match"])["query"]) = query;
                (((Dictionary<string, Object>)((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["query"])["multi_match"])["fields"]) = columns;
            }
            else
            {
                //if columns is an array.
                ((((Dictionary<string, Object>)dictionary["query"]))["multi_match"]) = new Dictionary<string, object>();
                (((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["multi_match"])["query"]) = query;
                (((Dictionary<string, Object>)((Dictionary<string, Object>)dictionary["query"])["multi_match"])["fields"]) = columns;
            }

            return this;
        }


        public async Task<ArrayList> Search()
        {
            var result = await Util.CloudRequest.POST("/search", dictionary);
            return (ArrayList)result;
        }
        public static CloudSearch Or(CloudSearch searchObj1, CloudSearch searchObj2)
        {

            var collectionNames = new ArrayList();

            if (searchObj1.dictionary["collectionNames"].GetType() == typeof(ArrayList))
            {
                collectionNames.AddRange((ArrayList)searchObj1.dictionary["collectionNames"]);
            }
            else
            {
                collectionNames.Add((ArrayList)searchObj1.dictionary["collectionNames"]);
            }

            if (searchObj2.dictionary["collectionNames"].GetType() == typeof(ArrayList))
            {
                //check for duplicates.
                for (var i = 0; i < ((ArrayList)searchObj2.dictionary["collectionNames"]).Count; i++)
                {
                    if (collectionNames.IndexOf(((ArrayList)searchObj2.dictionary["collectionNames"])[i]) < 0)
                        collectionNames.Add(((ArrayList)searchObj2.dictionary["collectionNames"])[i]);
                }
            }
            else
            {
                if (collectionNames.IndexOf(((string)searchObj2.dictionary["collectionNames"])) < 0)
                    collectionNames.Add(((ArrayList)searchObj2.dictionary["collectionNames"]));
            }

            var obj3 = new CB.CloudSearch(collectionNames);
            //merge both of the objects.

            Dictionary<string, Object> q1 = null;
            Dictionary<string, Object> q2 = null;
            Dictionary<string, Object> f1 = null;
            Dictionary<string, Object> f2 = null;

            if (((((Dictionary<string, Object>)searchObj1.dictionary["query"])["filteredQuery"])) != null && (((Dictionary<string, Object>)((Dictionary<string, Object>)searchObj1.dictionary["query"])["filteredQuery"])["query"]) != null)
            {
                q1 = (Dictionary<string, Object>)(((Dictionary<string, Object>)((Dictionary<string, Object>)searchObj1.dictionary["query"])["filteredQuery"])["query"]);
            }
            else if (searchObj1.dictionary["query"] != null && ((((Dictionary<string, Object>)searchObj1.dictionary["query"])["filteredQuery"])) == null)
            {
                q1 = (Dictionary<string, Object>)searchObj1.dictionary["query"];
            }

            if (((((Dictionary<string, Object>)searchObj2.dictionary["query"])["filteredQuery"])) != null && (((Dictionary<string, Object>)((Dictionary<string, Object>)searchObj2.dictionary["query"])["filteredQuery"])["query"]) != null)
            {
                q2 = (Dictionary<string, Object>)(((Dictionary<string, Object>)((Dictionary<string, Object>)searchObj2.dictionary["query"])["filteredQuery"])["query"]);
            }
            else if (searchObj2.dictionary["query"] != null && ((((Dictionary<string, Object>)searchObj2.dictionary["query"])["filteredQuery"])) == null)
            {
                q2 = (Dictionary<string, Object>)searchObj2.dictionary["query"];
            }


            if (((((Dictionary<string, Object>)searchObj1.dictionary["query"])["filteredQuery"])) != null && (((Dictionary<string, Object>)((Dictionary<string, Object>)searchObj1.dictionary["query"])["filteredQuery"])["filter"]) != null)
                f1 = (Dictionary<string, Object>)(((Dictionary<string, Object>)((Dictionary<string, Object>)searchObj1.dictionary["query"])["filteredQuery"])["filter"]);

            if (((((Dictionary<string, Object>)searchObj2.dictionary["query"])["filteredQuery"])) != null && (((Dictionary<string, Object>)((Dictionary<string, Object>)searchObj2.dictionary["query"])["filteredQuery"])["filter"]) != null)
                f2 = (Dictionary<string, Object>)(((Dictionary<string, Object>)((Dictionary<string, Object>)searchObj2.dictionary["query"])["filteredQuery"])["filter"]);

            if (f1.Count > 0 || f2.Count > 0)
            { //if any of the filters exist, then...
                obj3._MakeFilteredQuery();
                if (f1.Count > 0 && f2.Count == 0)
                    (((Dictionary<string, Object>)((Dictionary<string, Object>)obj3.dictionary["query"])["filteredQuery"])["filter"]) = f1;
                else if (f2.Count > 0 && f1.Count == 0)
                    (((Dictionary<string, Object>)((Dictionary<string, Object>)obj3.dictionary["query"])["filteredQuery"])["filter"]) = f2;
                else
                {
                    //if both exists.
                    obj3._PushInShouldFilter(f1);
                    obj3._PushInShouldFilter(f2);
                }
            }
            else
            {
                //only query exists.
                obj3._CreateBoolQuery();
                obj3._PushInShouldQuery(q1);
                obj3._PushInShouldQuery(q2);
            }

            return obj3;

        }
    }
}
