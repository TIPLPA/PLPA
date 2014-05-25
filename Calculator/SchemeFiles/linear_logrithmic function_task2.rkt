;;================================================================================================
;;     [SUB] Utility functions(subfunctions to supporting works for linear , logarimic functions)
;;
;; 1. "zip" function : to help make dataset (x . y) - used for both linear, logarimic 
;; 2. "nOfPoints-func" function : to calculate how many data points should be return 
;; 3. "LogB"
;;================================================================================================

;------------- "zip" from previous assignment ---------------------------
;[Description] zip function to make dataset (x . y) style
;<test1> (zip '(1 2 3) '(a b c))
; <oupput1>     => ((1 . a) (2 . b) (3 . c))
;<test2>(zip '(1 2) '(a b c))
; <oupput2>     => ((1 . a) (2 . b))

(define (zip lst1 lst2)
  (define (zip-helper ls1 ls2 acc)                              ;; use helper function for inner logic(intermediate work)
      (if (or (null? ls1) (null? ls2))                          ;; if one of input lists are empty
          (reverse acc)                                         ;; (then return as it is, but the final result is backwards order, so the final result need to be reversed)
          (zip-helper (cdr ls1) (cdr ls2)                       ;; otherwise do the logic to make pairs from the input lists
                    (cons (cons (car ls1) (car ls2)) acc))))
    (zip-helper lst1 lst2 '()))                                 ;; call yourself (tail recursion)

;------------- "nOfPoints" function  ---------------------------
;[Description] Utility function to calculate how many datapoints are needed in the given range and scale
; <test> (nOfPoints-func -10 10 0.1) 
; <output>      => 200.0
;[Param] xMin: Lowest value
;[Param] xMax: Highest value
; scale : scale for the between
(define nOfPoints-func
  (lambda (xMin xMax scale)
    (/ (- xMax xMin) scale)
    )
  )

;------------- "logB" function  ---------------------------
;[Description] Extedned Log function
;  <test> (logB 100 10) 
;  <oupput>   => 2
;[Param] x: value that we want the logarithem of
;[Param] b: the base of the logarithem. fx 10
(define logB 
  (lambda (x b)
        (/ (log x) (log b))
     ;)   
    )
  )

;------------- "logspace-func" function  ---------------------------
;[Description] logspace-func funciton from matlab with exponential
;  <test>  (logspace-func -2 1 10 1) 
;  <output> => 0.021544346900318832
;[Param] a : min. decade
;[Param] b : max. decade
;[Param] n : Number of points
;[Param] x : The specific point needed
(define logspace-func
  (lambda (a b n x)
    (expt 10 (+ a (/ (* x (- b a)) (- n 1))))
           )
  )
;------------- "adapter-logfunc" function  ---------------------------
;[Description] Adapter funciton for calling logspace with correct parameters
;  <test> (adapter-logfunc 0 10 10 1) 
;  <output>  =>  0.021544346900318857
;[Param] min : min. value
;[Param] max : max. value
;[Param] n : Number of points
;[Param] x : The specific point needed
(define adapter-logfunc
  (lambda (min max n x)         
        (logspace-func
         (cond ((= min 0)    (logB 0.01 10))            
               ((< min 0)    (logB (* min -1) 10))
               (else         (logB min 10))) 
         
         (cond ((= max 0)    (logB 0.01 10))
               ((< max 0)    (logB (* max -1) 10))
               (else         (logB max 10))) 
         n 
         x)
    )
  )

;; ============================================================================================================
;;               [GOAL]  linear, logarithmic Functions to use 
;; 
;; ============================================================================================================

;---------------[ linear function ] -----------------------------------------------------------------------------
;[Description] Function for generating a X and Y dataset with Linear X Y distribution '((x1 . y1) (x2 . y2) (...more...))
;  <test> (linearFunc 0 10 1 (lambda (x) x)) 
;  <output> => ((0 . 0) (1 . 1) (2 . 2) (3 . 3) (4 . 4) (5 . 5) (6 . 6) (7 . 7) (8 . 8) (9 . 9) (10 . 10))
;[param] start : minimum value
;[param] end   : maximum value
;[param] scale : scale of the linear
;[param] func  : A lambda function to genrate Y ( y = (func x)), this will use zip for return (x . y) 
(define linearFunc
  (lambda (start end scale func)
    (letrec ((helper-linear
              (lambda (x i y)
                (if (>= i start)           ;; if in the boundary , do the recursion process
                    (helper-linear (cons i x)           ;; recursion for x
                                   (- i scale)          ;; recursion for i
                                   (cons (func i ) y))  ;; recursion for y       
                  (zip x y)          ;; return the dataset values with style (x . y) by using subfunction "zip"
                 )
                )              
              ))
      (helper-linear '() end  '())         ;; kick off tail recursion for the function itself  
      )
    )
  )

;-----------------[ logarithmic function ] ---------------------------------------------------------------------------------
;[Description] Function for generating a X and Y dataset with logarithmic  X distribution '((x1 . y1) (x2 . y2) .....)
;  <test> (logarithmicFunc 0 10 1 (lambda (x) x))
;  <output> => ((4.9406564584125e-324 . 4.9406564584125e-324)  ...   (1.0 . 1.0) ...  (4.6415888336127775 . 4.6415888336127775) (10.10))
;[param] min      : minimun value
;[param] max      : maximum value
;[param] logscale : scale of logarithm 
;[param] func2    : A lambda function to genrate Y ( y = (func x))
(define logarithmicFunc
  (lambda (min max logscale func)
    (letrec ((helper-log
              (lambda (lstX lstY x i acc)                  
                (if (> x min)                             ;; if in the boundary, do the recursive process
                        (helper-log (cons x lstX)                            ;; recursion for lstX - do I need to check max every time? or it is okay to have in kick off place
                                    (cons (func x ) lstY)                    ;; recursion for lstY
                                    (adapter-logfunc min max acc i)          ;; recursion for x as the logarithm logic calling subfunctions for the math
                                    (- i 1)                                  ;; recursion for i 
                                    acc)
                    
                    (zip lstX lstY)         ;; return the dataset values with style (x . y) by using subfunction "zip"      
                   )
                )
              ))
      (helper-log '() '() max (- (nOfPoints-func min max logscale) 2) (nOfPoints-func min max logscale))  ;; kick off tail recursion for the function itself
      )
    )
  )
