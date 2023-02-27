.equ	newGameOrTable, newGameTable+4
.equ	newGameAndTable, newGameOrTable+4
.equ	return, newGameAndTable+4
.thumb
push	{r0-r7}

ldr	r4,newGameTable
bigloop:
ldr	r5,[r4]
cmp	r5,#0
beq	Or
ldr	r6,[r4,#4]
ldr	r7,[r4,#8]
smallloop:
cmp	r7,#0
beq	nextbigloop
ldrb	r0,[r5]
strb	r0,[r6]
sub	r7,#1
add	r5,#1
add	r6,#1
b	smallloop
nextbigloop:
add	r4,#12
b	bigloop

Or:
ldr	r4,newGameOrTable
bigloopor:
ldr	r5,[r4]
cmp	r5,#0
beq	And
ldr	r6,[r4,#4]
ldr	r7,[r4,#8]
smallloopor:
cmp	r7,#0
beq	nextbigloopor
ldrb	r0,[r5]
ldrb	r1,[r6]
orr	r0,r1
strb	r0,[r6]
sub	r7,#1
add	r5,#1
add	r6,#1
b	smallloopor
nextbigloopor:
add	r4,#12
b	bigloopor

And:
ldr	r4,newGameAndTable
bigloopand:
ldr	r5,[r4]
cmp	r5,#0
beq	End
ldr	r6,[r4,#4]
ldr	r7,[r4,#8]
smallloopand:
cmp	r7,#0
beq	nextbigloopand
ldrb	r0,[r5]
ldrb	r1,[r6]
and	r0,r1
strb	r0,[r6]
sub	r7,#1
add	r5,#1
add	r6,#1
b	smallloopand
nextbigloopand:
add	r4,#12
b	bigloopand

End:
ldr	r3,return
mov	lr,r3
pop	{r0-r7}
.short	0xF800

.align
.ltorg
newGameTable:
@POIN newGameTable
@POIN newGameOrTable
@POIN newGameAndTable
@POIN return
