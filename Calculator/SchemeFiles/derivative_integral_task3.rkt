;===========================================================================
;                         Derivative 
;===========================================================================
;----------- subfunction for derivative ---------------------------
;((derivative (lambda (x) (* 3 x))) 4)
(define dx 0.001)

(define (derivative-math f)
  (lambda (x)
    (/ (- (f (+ x dx)) (f x))
       dx)))

;----- zip 
(define (zip lst1 lst2)
  (define (zip-helper ls1 ls2 acc)                              ;; use helper function for inner logic(intermediate work)
      (if (or (null? ls1) (null? ls2))                          ;; if one of input lists are empty
          (reverse acc)                                         ;; (then return as it is, but the final result is backwards order, so the final result need to be reversed)
          (zip-helper (cdr ls1) (cdr ls2)                       ;; otherwise do the logic to make pairs from the input lists
                    (cons (cons (car ls1) (car ls2)) acc))))
    (zip-helper lst1 lst2 '())) 
;------------- Derivative----------------------------------------
;; ;; <test>   (derivativeFunc 0 10 1 (lambda (x) (* 3 x)))
(define derivativeFunc
  (lambda (min max count func)
    (letrec ( (interval (/ (- max min) count)) 
             (helper-log
              (lambda (lstX x lstY)                  
                (if (>= x min)                             ;; if in the boundary, do the recursive process
                        (helper-log (cons x lstX)                            ;; recursion for lstX - do I need to check max every time? or it is okay to have in kick off place
                                    (- x interval)                                  ;; recursion for i 
                                    (cons ((derivative-math func) x) lstY)         ;; recursion for x as the logarithm logic calling subfunctions for the math
                                    )
                    
                    (zip lstX lstY)            ;; return the dataset values with style (x . y) by using subfunction "zip"      
                   )
                )
              ))
      (helper-log '() max '())  ;; kick off tail recursion for the function itself
      )
    )
  )
;===========================================================================
;                         Integral 
;===========================================================================
;;[Description] Integral function use "rectangle method"
;;“rectangle method” : sum itself can be used as a building block for example 
;;  to approximate definite integrals by summing area of small rectangles integral from a to b of f =
;;[math]   ====>   [f(a + dx/2) + f(a + dx + dx/2) + f(a + 2dx + dx/2)  + ... + f(b - dx)] * dx

;-----  sum for "rectangle method" -----------------------------------------------------------
(define (sum term x next y)
  (if (> x y)
      0
      (+ (term x)
         (sum term (next x) next y))))

;----- integral -------------------------
(define (integral func a b dx)
  (* (sum func
          (+ a (/ dx 2.0))
               (lambda (x) (+ x dx))
                b)
     dx))
