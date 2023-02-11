.thumb
push	{r4-r6,lr}
mov	r4,#0x40
mov	r5,#0
ldr	r6,=#0x807C4A8
loop:
mov	r0,r4
mov	lr,r6
.short	0xF800
cmp	r0,#0
beq	nope
add	r5,#1
nope:
add	r4,#1
cmp	r4,#0x44
bne	loop

ldr	r0,ammount
cmp	r0,#4
bhi	max
check:
cmp	r5,r0
blo	false
b	true

false:
mov	r0,#0
pop	{r4-r6,pc}

true:
mov	r0,#1
pop	{r4-r6,pc}

max:
mov	r0,#4
b	check

.align
.ltorg
ammount:
