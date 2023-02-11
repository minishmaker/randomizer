.thumb
@ cap hp
ldr	r3, heartLimited
cmp	r0, r3
blo	noCap
mov	r0, r3
noCap:

@ and return to vanilla code
cmp	r0, r1
bge	end
mov	r1, r0
end:
strb	r1, [r2, #2]
ldr	r3, =#0x80522D4
mov	lr, r3
.short	0xF800

.align
.ltorg
heartLimited: